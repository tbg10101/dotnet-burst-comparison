using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace DotNetBurstComparison.Unity.Benchmarks {
    /// <summary>
    /// This is supposed to test quaternion multiplication.
    /// https://docs.unity3d.com/Packages/com.unity.mathematics@1.3/manual/quaternion-multiplication.html
    /// </summary>
    public sealed class QuaternionMultiplication: IBenchmark {
        private const int ArrayLength = 1_000_000; // 1_000_000

        private readonly Quaternion[] _quaternionArrayA = new Quaternion[ArrayLength];
        private readonly Quaternion[] _quaternionArrayB = new Quaternion[ArrayLength];

        private readonly NativeArray<quaternion> _quaternionNativeArrayA = new(ArrayLength, Allocator.Persistent);
        private readonly NativeArray<quaternion> _quaternionNativeArrayB = new(ArrayLength, Allocator.Persistent);

        public QuaternionMultiplication() {
            NativeArray<quaternion> nativeArrayA = _quaternionNativeArrayA;
            NativeArray<quaternion> nativeArrayB = _quaternionNativeArrayB;

            float3 axis = new(0.0f, 1.0f, 0.0f);

            for (int i = 0; i < ArrayLength; i++) {
                _quaternionArrayA[i] = nativeArrayA[i] = quaternion.AxisAngle(axis, i % 2.0f * math.PI);
                _quaternionArrayB[i] = nativeArrayB[i] = quaternion.AxisAngle(axis, (i + 0.125f * math.PI) % 2.0f * math.PI);
            }
        }

        public void Dispose() {
            // ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable
            _quaternionNativeArrayA.Dispose();
            _quaternionNativeArrayB.Dispose();
            // ReSharper enable PossiblyImpureMethodCallOnReadonlyVariable
        }

        [BurstDiscard]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void RunNonBurst() {
            Quaternion[] a = _quaternionArrayA;
            Quaternion[] b = _quaternionArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                a[i] *= b[i];
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void RunBurst() {
            NativeArray<quaternion> a = _quaternionNativeArrayA;
            NativeArray<quaternion> b = _quaternionNativeArrayB;

            LoopVectorizationJob job = new() {
                A = a,
                B = b
            };

            job.Schedule().Complete();
        }

        [BurstCompile]
        private struct LoopVectorizationJob : IJob {
            public NativeArray<quaternion> A;

            [ReadOnly]
            public NativeArray<quaternion> B;

            public void Execute() {
                int length = A.Length;

                for (int i = 0; i < length; i++) {
                    A[i] = math.mul(A[i], B[i]);
                }
            }
        }
    }
}
