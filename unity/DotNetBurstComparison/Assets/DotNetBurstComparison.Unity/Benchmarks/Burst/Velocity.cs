using System.Runtime.CompilerServices;
using DotNetBurstComparison.Runner;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace DotNetBurstComparison.Unity.Benchmarks.Burst {
    /// <summary>
    /// This is supposed to test a real-world ECS use-case where velocity multiplied by a time delta is added to positions.
    /// </summary>
    public sealed class Velocity : IBenchmark {
        private const int ArrayLength = 1_000_000;
        private const float TimeDelta = 0.033f;

        private readonly NativeArray<float4> _vectorNativeArrayA = new(ArrayLength, Allocator.Persistent);
        private readonly NativeArray<float4> _vectorNativeArrayB = new(ArrayLength, Allocator.Persistent);

        public Velocity() {
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
                    A[i] += TimeDelta * B[i];
                }
            }
        }
    }
}
