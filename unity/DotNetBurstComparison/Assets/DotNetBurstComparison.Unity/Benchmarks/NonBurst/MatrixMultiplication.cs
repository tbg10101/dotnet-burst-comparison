using System.Runtime.CompilerServices;
using DotNetBurstComparison.Runner;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace DotNetBurstComparison.Unity.Benchmarks.NonBurst {
    /// <summary>
    /// This is supposed to test matrix multiplication.
    /// https://docs.unity3d.com/Packages/com.unity.mathematics@1.3/manual/4x4-matrices.html
    /// </summary>
    public sealed class MatrixMultiplication : IBenchmark {
        private const int ArrayLength = 1_000_000;

        private readonly Matrix4x4[] _vectorArrayA = new Matrix4x4[ArrayLength];
        private readonly Matrix4x4[] _vectorArrayB = new Matrix4x4[ArrayLength];

        public MatrixMultiplication() {
           float3 axis = new(0.0f, 1.0f, 0.0f);

            for (int i = 0; i < ArrayLength; i++) {
                _vectorArrayA[i] = float4x4.TRS(
                    new float3(math.sin(3 * i + 0), math.sin(3 * i + 1), math.sin(3 * i + 2)),
                    quaternion.AxisAngle(axis, i % 2.0f * math.PI),
                    new float3(1.0f, 1.0f, 1.0f)
                );
                _vectorArrayB[i] = float4x4.TRS(
                    new float3(math.cos(3 * i + 0), math.cos(3 * i + 1), math.cos(3 * i + 2)),
                    quaternion.AxisAngle(axis, i % 2.0f * math.PI),
                    new float3(1.0f, 1.0f, 1.0f)
                );
            }
        }

        public void Dispose() {
            // do nothing
        }

        [BurstDiscard]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public Result Run() {
            Matrix4x4[] a = _vectorArrayA;
            Matrix4x4[] b = _vectorArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                a[i] *= b[i];
            }

            return default;
        }
    }
}
