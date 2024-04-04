using System;
using System.Runtime.CompilerServices;
using DotNetBurstComparison.Runner;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace DotNetBurstComparison.Unity.Benchmarks.Burst {
    /// <summary>
    /// This is supposed to test loop vectorization.
    /// https://docs.unity3d.com/Packages/com.unity.burst@1.8/manual/optimization-loop-vectorization.html
    /// </summary>
    public sealed class LoopVectorization : IBenchmark {
        private const int ArrayLength = 1_000_000;

        private readonly NativeArray<float> _floatNativeArrayA = new(ArrayLength, Allocator.Persistent);
        private readonly NativeArray<float> _floatNativeArrayB = new(ArrayLength, Allocator.Persistent);

        public LoopVectorization() {
            NativeArray<float> nativeArrayA = _floatNativeArrayA;
            NativeArray<float> nativeArrayB = _floatNativeArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                nativeArrayA[i] = (float)Math.Sin(i);
                nativeArrayB[i] = (float)Math.Cos(i);
            }
        }

        public void Dispose() {
            // ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable
            _floatNativeArrayA.Dispose();
            _floatNativeArrayB.Dispose();
            // ReSharper enable PossiblyImpureMethodCallOnReadonlyVariable
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public Result Run() {
            NativeArray<float> a = _floatNativeArrayA;
            NativeArray<float> b = _floatNativeArrayB;

            LoopVectorizationJob job = new() {
                A = a,
                B = b
            };

            job.Schedule().Complete();

            return default;
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
