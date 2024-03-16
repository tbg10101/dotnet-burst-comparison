using BenchmarkDotNet.Attributes;
using Unity.Burst;
using Unity.Jobs;

namespace DotNetBurstComparison.Unity.Benchmarks {
    /// <summary>
    /// Shamelessly borrowed: https://github.com/nxrighthere/BurstBenchmarks
    /// </summary>
    [SimpleJob]
    [IterationsColumn]
    public class Fibonacci {
        private const uint Number = 46;

        [BurstDiscard]
        [Benchmark]
        [BenchmarkCategory("NonBurst")]
        public void RunNonBurst() {
            uint _ = DoFibonacci(Number);
        }

        [Benchmark]
        [BenchmarkCategory("Burst")]
        public void RunBurst() {
            FibonacciJob job = new() {
                Number = Number
            };

            job.Schedule().Complete();
        }

        [BurstCompile]
        private struct FibonacciJob : IJob {
            public uint Number;
            public uint Result;

            public void Execute() {
                Result = DoFibonacci(Number);
            }
        }

        private static uint DoFibonacci(uint number) {
            if (number <= 1)
                return 1;

            return DoFibonacci(number - 1) + DoFibonacci(number - 2);
        }
    }
}
