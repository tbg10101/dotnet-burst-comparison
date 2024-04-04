using System.Runtime.CompilerServices;
using DotNetBurstComparison.Runner;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace DotNetBurstComparison.Unity.Benchmarks.NonBurst {
    /// <summary>
    /// This is supposed to test quaternion multiplication.
    /// https://docs.unity3d.com/Packages/com.unity.mathematics@1.3/manual/quaternion-multiplication.html
    /// </summary>
    public sealed class QuaternionMultiplication : IBenchmark {
        private const int ArrayLength = 1_000_000; // 1_000_000

        private readonly Quaternion[] _quaternionArrayA = new Quaternion[ArrayLength];
        private readonly Quaternion[] _quaternionArrayB = new Quaternion[ArrayLength];

        public QuaternionMultiplication() {
            float3 axis = new(0.0f, 1.0f, 0.0f);

            for (int i = 0; i < ArrayLength; i++) {
                _quaternionArrayA[i] = quaternion.AxisAngle(axis, i % 2.0f * math.PI);
                _quaternionArrayB[i] = quaternion.AxisAngle(axis, (i + 0.125f * math.PI) % 2.0f * math.PI);
            }
        }

        public void Dispose() {
            // do nothing
        }

        [BurstDiscard]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public Result Run() {
            Quaternion[] a = _quaternionArrayA;
            Quaternion[] b = _quaternionArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                a[i] *= b[i];
            }

            return default;
        }
    }
}
