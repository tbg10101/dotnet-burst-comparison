using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Jobs;

namespace DotNetBurstComparison.Unity.Benchmarks {
    /// <summary>
    /// Shamelessly borrowed: https://github.com/nxrighthere/BurstBenchmarks
    /// </summary>
    public sealed class Fibonacci : IBenchmark {
        private const uint Number = 38; // 46

        public Fibonacci() {
            // do nothing
        }

        public void Dispose() {
            // do nothing
        }

        [BurstDiscard]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void RunNonBurst() {
            uint result = DoFibonacci(Number);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
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
