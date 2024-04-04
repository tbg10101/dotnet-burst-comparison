using System.Runtime.CompilerServices;
using DotNetBurstComparison.Runner;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace DotNetBurstComparison.Unity.Benchmarks.Burst {
    /// <summary>
    /// This is supposed to test matrix multiplication.
    /// https://docs.unity3d.com/Packages/com.unity.mathematics@1.3/manual/4x4-matrices.html
    /// </summary>
    public sealed class MatrixMultiplication : IBenchmark {
        private const int ArrayLength = 1_000_000;

        private readonly NativeArray<float4x4> _vectorNativeArrayA = new(ArrayLength, Allocator.Persistent);
        private readonly NativeArray<float4x4> _vectorNativeArrayB = new(ArrayLength, Allocator.Persistent);

        public MatrixMultiplication() {
            NativeArray<float4x4> nativeArrayA = _vectorNativeArrayA;
            NativeArray<float4x4> nativeArrayB = _vectorNativeArrayB;

            float3 axis = new(0.0f, 1.0f, 0.0f);

            for (int i = 0; i < ArrayLength; i++) {
                nativeArrayA[i] = float4x4.TRS(
                    new float3(math.sin(3 * i + 0), math.sin(3 * i + 1), math.sin(3 * i + 2)),
                    quaternion.AxisAngle(axis, i % 2.0f * math.PI),
                    new float3(1.0f, 1.0f, 1.0f)
                );
                nativeArrayB[i] = float4x4.TRS(
                    new float3(math.cos(3 * i + 0), math.cos(3 * i + 1), math.cos(3 * i + 2)),
                    quaternion.AxisAngle(axis, i % 2.0f * math.PI),
                    new float3(1.0f, 1.0f, 1.0f)
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
            NativeArray<float4x4> a = _vectorNativeArrayA;
            NativeArray<float4x4> b = _vectorNativeArrayB;

            LoopVectorizationJob job = new() {
                A = a,
                B = b
            };

            job.Schedule().Complete();

            return default;
        }

        [BurstCompile]
        private struct LoopVectorizationJob : IJob {
            public NativeArray<float4x4> A;

            [ReadOnly]
            public NativeArray<float4x4> B;

            public void Execute() {
                int length = A.Length;

                for (int i = 0; i < length; i++) {
                    A[i] *= B[i];
                }
            }
        }
    }
}
