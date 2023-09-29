using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace DotNetBurstComparison.Unity.Benchmarks {
    /// <summary>
    /// This is supposed to test vector multiplication.
    /// https://docs.unity3d.com/Packages/com.unity.mathematics@1.3/manual/vector-multiplication.html
    /// </summary>
    public sealed class VectorMultiplication: IBenchmark {
        private const int ArrayLength = 1_000_000; // 1_000_000

        private readonly Vector4[] _vectorArrayA = new Vector4[ArrayLength];
        private readonly Vector4[] _vectorArrayB = new Vector4[ArrayLength];

        private readonly NativeArray<float4> _vectorNativeArrayA = new(ArrayLength, Allocator.Persistent);
        private readonly NativeArray<float4> _vectorNativeArrayB = new(ArrayLength, Allocator.Persistent);

        public VectorMultiplication() {
            NativeArray<float4> nativeArrayA = _vectorNativeArrayA;
            NativeArray<float4> nativeArrayB = _vectorNativeArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                _vectorArrayA[i] = nativeArrayA[i] = new float4(
                    (float)Math.Sin(4 * i + 0),
                    (float)Math.Sin(4 * i + 1),
                    (float)Math.Sin(4 * i + 2),
                    (float)Math.Sin(4 * i + 3)
                );
                _vectorArrayB[i] = nativeArrayB[i] = new float4(
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
            Vector4[] a = _vectorArrayA;
            Vector4[] b = _vectorArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                Vector4 vA = a[i];
                Vector4 vB = b[i];

                a[i] = new Vector4(vA.x * vB.x, vA.y * vB.y, vA.z * vB.z, vA.w * vB.w);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
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
                for (int i = 0; i < A.Length; i++) {
                    A[i] *= B[i];
                }
            }
        }
    }
}
