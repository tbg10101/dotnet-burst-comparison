using System;
using System.Collections.Generic;
using System.Diagnostics;
using DotNetBurstComparison.Unity.Benchmarks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DotNetBurstComparison.Unity {
    public static class Program {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Main() {
            PerformTests(new Func<IBenchmark>[] {
                () => new LoopVectorization(),
                () => new Fibonacci(),
                () => new Mandelbrot(),
                () => new SieveOfEratosthenes(),
                () => new VectorMultiplication(),
                () => new QuaternionMultiplication(),
                () => new MatrixMultiplication(),
                () => new Velocity(),
            });
        }

        private static void PerformTests(IReadOnlyList<Func<IBenchmark>> benchmarkingInitializers) {
            foreach (Func<IBenchmark> initializer in benchmarkingInitializers) {
                TestResult result = PerformTest(initializer);
                Debug.Log(result);
            }
        }

        private static TestResult PerformTest(Func<IBenchmark> benchmarkingInitializer) {
            IBenchmark benchmark = benchmarkingInitializer();

            string testName = benchmark.GetType().Name;
            Stopwatch sw = new();

            sw.Restart();
            benchmark.RunNonBurst();
            sw.Stop();
            TimeSpan nonBurstElapsedInitial = sw.Elapsed;

            sw.Restart();
            benchmark.RunNonBurst();
            sw.Stop();
            TimeSpan nonBurstElapsed = sw.Elapsed;

            sw.Restart();
            benchmark.RunBurst();
            sw.Stop();
            TimeSpan burstElapsedInitial = sw.Elapsed;

            sw.Restart();
            benchmark.RunBurst();
            sw.Stop();
            TimeSpan burstElapsed = sw.Elapsed;

            benchmark.Dispose();

            return new TestResult(
                testName,
                nonBurstElapsedInitial,
                nonBurstElapsed,
                burstElapsedInitial,
                burstElapsed);
        }
    }

    public readonly struct TestResult {
        public readonly string TestName;

        public readonly TimeSpan ElapsedTimeNonBurstInitial;
        public readonly TimeSpan ElapsedTimeNonBurst;

        public readonly TimeSpan ElapsedTimeBurstInitial;
        public readonly TimeSpan ElapsedTimeBurst;

        public TestResult(
            string testName,
            TimeSpan elapsedTimeNonBurstInitial,
            TimeSpan elapsedTimeNonBurst,
            TimeSpan elapsedTimeBurstInitial,
            TimeSpan elapsedTimeBurst
        ) {
            TestName = testName;

            ElapsedTimeNonBurstInitial = elapsedTimeNonBurstInitial;
            ElapsedTimeNonBurst = elapsedTimeNonBurst;

            ElapsedTimeBurstInitial = elapsedTimeBurstInitial;
            ElapsedTimeBurst = elapsedTimeBurst;
        }

        public override string ToString() {
            return $"{TestName}\n" +
                   $"    Non-Burst: {FormatTimeSpan(ElapsedTimeNonBurstInitial)} | {FormatTimeSpan(ElapsedTimeNonBurst)}\n" +
                   $"    Burst: {FormatTimeSpan(ElapsedTimeBurstInitial)} | {FormatTimeSpan(ElapsedTimeBurst)}";
        }

        private static string FormatTimeSpan(TimeSpan duration) {
            return duration.Ticks.ToString("N0");
        }
    }
}
