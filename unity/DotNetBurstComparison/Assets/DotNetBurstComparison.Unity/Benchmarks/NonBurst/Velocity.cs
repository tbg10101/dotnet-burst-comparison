using System.Runtime.CompilerServices;
using DotNetBurstComparison.Runner;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace DotNetBurstComparison.Unity.Benchmarks.NonBurst {
    /// <summary>
    /// This is supposed to test a real-world ECS use-case where velocity multiplied by a time delta is added to positions.
    /// </summary>
    public sealed class Velocity : IBenchmark {
        private const int ArrayLength = 1_000_000;
        private const float TimeDelta = 0.033f;

        private readonly Vector4[] _vectorArrayA = new Vector4[ArrayLength];
        private readonly Vector4[] _vectorArrayB = new Vector4[ArrayLength];

        public Velocity() {
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
                a[i] += TimeDelta * b[i];
            }

            return default;
        }
    }
}
