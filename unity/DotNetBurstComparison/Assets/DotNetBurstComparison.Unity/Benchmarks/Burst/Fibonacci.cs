using System.Runtime.CompilerServices;
using DotNetBurstComparison.Runner;
using Unity.Burst;
using Unity.Jobs;

namespace DotNetBurstComparison.Unity.Benchmarks.Burst {
    /// <summary>
    /// Shamelessly borrowed: https://github.com/nxrighthere/BurstBenchmarks
    /// </summary>
    public sealed class Fibonacci : IBenchmark {
        private const uint Number = 46;

        public Fibonacci() {
            // do nothing
        }

        public void Dispose() {
            // do nothing
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public Result Run() {
            FibonacciJob job = new() {
                Number = Number
            };

            job.Schedule().Complete();

            return job.Result;
        }

        [BurstCompile]
        private struct FibonacciJob : IJob {
            public uint Number;
            public uint Result;

            public void Execute() {
                Result = DoFibonacci(Number);
            }

            private static uint DoFibonacci(uint number) {
                if (number <= 1)
                    return 1;

                return DoFibonacci(number - 1) + DoFibonacci(number - 2);
            }
        }
    }
}
