using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace DotNetBurstComparison.unity {
    public static class Tests {
        private const int ArrayLength = 100_000_000;

        private static readonly float[] FloatArrayA = new float[ArrayLength];
        private static readonly float[] FloatArrayB = new float[ArrayLength];

        private static readonly NativeArray<float> FloatNativeArrayA = new(ArrayLength, Allocator.Persistent);
        private static readonly NativeArray<float> FloatNativeArrayB = new(ArrayLength, Allocator.Persistent);

        public static void Initialize() {
            NativeArray<float> nativeArrayA = FloatNativeArrayA;
            NativeArray<float> nativeArrayB = FloatNativeArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                FloatArrayA[i] = nativeArrayA[i] = (float)Math.Sin(i);
                FloatArrayB[i] = nativeArrayB[i] = (float)Math.Cos(i);
            }
        }

        public static void CleanUp() {
            // ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable
            FloatNativeArrayA.Dispose();
            FloatNativeArrayB.Dispose();
            // ReSharper enable PossiblyImpureMethodCallOnReadonlyVariable
        }

        /// <summary>
        /// This is supposed to test loop vectorization:
        /// https://docs.unity3d.com/Packages/com.unity.burst@1.8/manual/optimization-loop-vectorization.html
        /// </summary>
        public static void ArrayElementAdditionBurst() {
            NativeArray<float> a = FloatNativeArrayA;
            NativeArray<float> b = FloatNativeArrayB;

            ArrayElementAdditionBurstJob job = new() {
                A = a,
                B = b
            };
            job.Schedule().Complete();
        }

        /// <summary>
        /// Baseline for loop vectorization.
        /// </summary>
        [BurstDiscard]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ArrayElementAdditionNoBurst() {
            NativeArray<float> a = FloatNativeArrayA;
            NativeArray<float> b = FloatNativeArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                a[i] += b[i];
            }
        }

        /// <summary>
        /// Another baseline for loop vectorization.
        /// </summary>
        [BurstDiscard]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ArrayElementAdditionNoBurstNonNative() {
            float[] a = FloatArrayA;
            float[] b = FloatArrayB;

            for (int i = 0; i < ArrayLength; i++) {
                a[i] += b[i];
            }
        }

        // TODO
    }

    [BurstCompile]
    public struct ArrayElementAdditionBurstJob : IJob {
        public NativeArray<float> A;

        [ReadOnly]
        public NativeArray<float> B;

        public void Execute() {
            for (int i = 0; i < A.Length; i++) {
                // Unity.Burst.CompilerServices.Loop.ExpectVectorized();
                A[i] += B[i];
            }
        }
    }
}
