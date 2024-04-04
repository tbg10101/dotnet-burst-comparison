using System.Runtime.CompilerServices;
using DotNetBurstComparison.Runner;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace DotNetBurstComparison.Unity.Benchmarks.NonBurst {
    /// <summary>
    /// This is supposed to test vector multiplication.
    /// https://docs.unity3d.com/Packages/com.unity.mathematics@1.3/manual/vector-multiplication.html
    /// </summary>
    public sealed class VectorMultiplication : IBenchmark {
        private const int ArrayLength = 1_000_000;

        private readonly Vector4[] _vectorArrayA = new Vector4[ArrayLength];
        private readonly Vector4[] _vectorArrayB = new Vector4[ArrayLength];

        public VectorMultiplication() {
            for (int i = 0; i < ArrayLength; i++) {
                _vectorArrayA[i] = new float4(
                    math.sin(4 * i + 0),
                    math.sin(4 * i + 1),
                    math.sin(4 * i + 2),
                    math.sin(4 * i + 3)
                );
                _vectorArrayB[i] = new float4(
                    math.cos(4 * i + 0),
                    math.cos(4 * i + 1),
                    math.cos(4 * i + 2),
                    math.cos(4 * i + 3)
                );
            }
        }

        public void Dispose() {
            // do nothing
        }

        [BurstDiscard]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public Result Run() {
            Vector4[] a = _vectorArrayA;
            Vector4[] b = _vectorArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                Vector4 vA = a[i];
                Vector4 vB = b[i];

                a[i] = new Vector4(vA.x * vB.x, vA.y * vB.y, vA.z * vB.z, vA.w * vB.w);
            }

            return default;
        }
    }
}
