using BenchmarkDotNet.Attributes;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace DotNetBurstComparison.Unity.Benchmarks {
    /// <summary>
    /// This is supposed to test a real-world ECS use-case where velocity multiplied by a time delta is added to positions.
    /// </summary>
    [SimpleJob]
    [IterationsColumn]
    public class Velocity {
        private const int ArrayLength = 1_000_000;
        private const float TimeDelta = 0.033f;

        private readonly Vector4[] _vectorArrayA = new Vector4[ArrayLength];
        private readonly Vector4[] _vectorArrayB = new Vector4[ArrayLength];

        private readonly NativeArray<float4> _vectorNativeArrayA = new(ArrayLength, Allocator.Persistent);
        private readonly NativeArray<float4> _vectorNativeArrayB = new(ArrayLength, Allocator.Persistent);

        public Velocity() {
            NativeArray<float4> nativeArrayA = _vectorNativeArrayA;
            NativeArray<float4> nativeArrayB = _vectorNativeArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                _vectorArrayA[i] = nativeArrayA[i] = new float4(
                    math.sin(4 * i + 0),
                    math.sin(4 * i + 1),
                    math.sin(4 * i + 2),
                    math.sin(4 * i + 3)
                );
                _vectorArrayB[i] = nativeArrayB[i] = new float4(
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

        [BurstDiscard]
        [Benchmark]
        [BenchmarkCategory("NonBurst")]
        public void RunNonBurst() {
            Vector4[] a = _vectorArrayA;
            Vector4[] b = _vectorArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                a[i] += TimeDelta * b[i];
            }
        }

        [Benchmark]
        [BenchmarkCategory("Burst")]
        public void RunBurst() {
            NativeArray<float4> a = _vectorNativeArrayA;
            NativeArray<float4> b = _vectorNativeArrayB;

            LoopVectorizationJob job = new() {
                A = a,
                B = b
            };

            job.Schedule().Complete();
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
