using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DotNetBurstComparison.unity {
    public static class Program {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Main() {
            Tests.Initialize();

            IReadOnlyList<TestResult> results = PerformTests(
                new Action[] {
                    Tests.ArrayElementAdditionBurst,
                    Tests.ArrayElementAdditionNoBurst,
                    Tests.ArrayElementAdditionNoBurstNonNative,
                },
                3);

            Tests.CleanUp();

            foreach (TestResult testResult in results) {
                Debug.Log(testResult);
            }
        }

        private static IReadOnlyList<TestResult> PerformTests(IReadOnlyList<Action> tests, uint iterations) {
            TestResult[] results = new TestResult[tests.Count];

            for (int i = 0; i < tests.Count; i++) {
                results[i] = PerformTest(tests[i], iterations);
            }

            return results;
        }

        private static TestResult PerformTest(Action test, uint iterations) {
            string testName = test.Method.Name;
            Stopwatch sw = new();
            TimeSpan elapsed = TimeSpan.Zero;
            TimeSpan initialElapsed = TimeSpan.Zero;

            for (int i = 0; i < iterations; i++) {
                sw.Restart();
                test();
                sw.Stop();

                elapsed += sw.Elapsed;

                if (i == 0) {
                    initialElapsed = elapsed;
                }
            }

            return new TestResult(testName, iterations, elapsed, initialElapsed);
        }
    }

    public readonly struct TestResult {
        public readonly string TestName;
        public readonly uint Iterations;
        public readonly TimeSpan ElapsedTime;
        public readonly TimeSpan InitialElapsedTime;

        public TestResult(string testName, uint iterations, TimeSpan elapsedTime, TimeSpan initialElapsedTime) {
            TestName = testName;
            Iterations = iterations;
            ElapsedTime = elapsedTime;
            InitialElapsedTime = initialElapsedTime;
        }

        public override string ToString() {
            return $"{TestName}: {ElapsedTime.Ticks / Iterations} (initial={InitialElapsedTime.Ticks})";
        }
    }
}
