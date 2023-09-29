using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace DotNetBurstComparison.Unity.Benchmarks {
    /// <summary>
    /// This is supposed to test matrix multiplication.
    /// https://docs.unity3d.com/Packages/com.unity.mathematics@1.3/manual/4x4-matrices.html
    /// </summary>
    public sealed class MatrixMultiplication: IBenchmark {
        private const int ArrayLength = 1_000_000; // 1_000_000

        private readonly Matrix4x4[] _vectorArrayA = new Matrix4x4[ArrayLength];
        private readonly Matrix4x4[] _vectorArrayB = new Matrix4x4[ArrayLength];

        private readonly NativeArray<float4x4> _vectorNativeArrayA = new(ArrayLength, Allocator.Persistent);
        private readonly NativeArray<float4x4> _vectorNativeArrayB = new(ArrayLength, Allocator.Persistent);

        public MatrixMultiplication() {
            NativeArray<float4x4> nativeArrayA = _vectorNativeArrayA;
            NativeArray<float4x4> nativeArrayB = _vectorNativeArrayB;

            for (int i = 0; i < ArrayLength; i++) { // TODO
                _vectorArrayA[i] = nativeArrayA[i] = float4x4.TRS(
                    new float3(),
                    new quaternion(),
                    new float3()
                );
                _vectorArrayB[i] = nativeArrayB[i] = new float4x4(
                    (float)Math.Cos(4 * i + 0),
                    (float)Math.Cos(4 * i + 1),
                    (float)Math.Cos(4 * i + 2),
                    (float)Math.Cos(4 * i + 3)
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
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void RunNonBurst() {
            Matrix4x4[] a = _vectorArrayA;
            Matrix4x4[] b = _vectorArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                a[i] *= b[i];
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void RunBurst() {
            NativeArray<float4x4> a = _vectorNativeArrayA;
            NativeArray<float4x4> b = _vectorNativeArrayB;

            LoopVectorizationJob job = new() {
                A = a,
                B = b
            };

            job.Schedule().Complete();
        }

        [BurstCompile]
        private struct LoopVectorizationJob : IJob {
            public NativeArray<float4x4> A;

            [ReadOnly]
            public NativeArray<float4x4> B;

            public void Execute() {
                for (int i = 0; i < A.Length; i++) {
                    A[i] *= B[i];
                }
            }
        }
    }
}
