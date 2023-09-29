using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace DotNetBurstComparison.Unity.Benchmarks {
    /// <summary>
    /// This is supposed to test loop vectorization.
    /// https://docs.unity3d.com/Packages/com.unity.burst@1.8/manual/optimization-loop-vectorization.html
    /// </summary>
    public sealed class LoopVectorization: IBenchmark {
        private const int ArrayLength = 1_000_000; // 1_000_000

        private readonly float[] _floatArrayA = new float[ArrayLength];
        private readonly float[] _floatArrayB = new float[ArrayLength];

        private readonly NativeArray<float> _floatNativeArrayA = new(ArrayLength, Allocator.Persistent);
        private readonly NativeArray<float> _floatNativeArrayB = new(ArrayLength, Allocator.Persistent);

        public LoopVectorization() {
            NativeArray<float> nativeArrayA = _floatNativeArrayA;
            NativeArray<float> nativeArrayB = _floatNativeArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                _floatArrayA[i] = nativeArrayA[i] = (float)Math.Sin(i);
                _floatArrayB[i] = nativeArrayB[i] = (float)Math.Cos(i);
            }
        }

        public void Dispose() {
            // ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable
            _floatNativeArrayA.Dispose();
            _floatNativeArrayB.Dispose();
            // ReSharper enable PossiblyImpureMethodCallOnReadonlyVariable
        }

        [BurstDiscard]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void RunNonBurst() {
            float[] a = _floatArrayA;
            float[] b = _floatArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                a[i] += b[i];
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void RunBurst() {
            NativeArray<float> a = _floatNativeArrayA;
            NativeArray<float> b = _floatNativeArrayB;

            LoopVectorizationJob job = new() {
                A = a,
                B = b
            };

            job.Schedule().Complete();
        }

        [BurstCompile]
        private struct LoopVectorizationJob : IJob {
            public NativeArray<float> A;

            [ReadOnly]
            public NativeArray<float> B;

            public void Execute() {
                for (int i = 0; i < A.Length; i++) {
                    A[i] += B[i];
                }
            }
        }
    }
}
