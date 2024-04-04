using System.Runtime.CompilerServices;
using DotNetBurstComparison.Runner;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace DotNetBurstComparison.Unity.Benchmarks.Burst {
    /// <summary>
    /// This is supposed to test vector multiplication.
    /// https://docs.unity3d.com/Packages/com.unity.mathematics@1.3/manual/vector-multiplication.html
    /// </summary>
    public sealed class VectorMultiplication : IBenchmark {
        private const int ArrayLength = 1_000_000;

        private readonly NativeArray<float4> _vectorNativeArrayA = new(ArrayLength, Allocator.Persistent);
        private readonly NativeArray<float4> _vectorNativeArrayB = new(ArrayLength, Allocator.Persistent);

        public VectorMultiplication() {
            NativeArray<float4> nativeArrayA = _vectorNativeArrayA;
            NativeArray<float4> nativeArrayB = _vectorNativeArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                nativeArrayA[i] = new float4(
                    math.sin(4 * i + 0),
                    math.sin(4 * i + 1),
                    math.sin(4 * i + 2),
                    math.sin(4 * i + 3)
                );
                nativeArrayB[i] = new float4(
                    math.cos(4 * i + 0),
                    math.cos(4 * i + 1),
                    math.cos(4 * i + 2),
                    math.cos(4 * i + 3)
                );
            }
        }

        public void Dispose() {
            // ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable
            _vectorNativeArrayA.Dispose();
            _vectorNativeArrayB.Dispose();
            // ReSharper enable PossiblyImpureMethodCallOnReadonlyVariable
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public Result Run() {
            NativeArray<float4> a = _vectorNativeArrayA;
            NativeArray<float4> b = _vectorNativeArrayB;

            LoopVectorizationJob job = new() {
                A = a,
                B = b
            };

            job.Schedule().Complete();

            return default;
        }

        [BurstCompile]
        private struct LoopVectorizationJob : IJob {
            public NativeArray<float4> A;

            [ReadOnly]
            public NativeArray<float4> B;

            public void Execute() {
                int length = A.Length;

                for (int i = 0; i < length; i++) {
                    A[i] *= B[i];
                }
            }
        }
    }
}
