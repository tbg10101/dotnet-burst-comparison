using System.Runtime.CompilerServices;
using DotNetBurstComparison.Runner;
using Unity.Burst;

namespace DotNetBurstComparison.Unity.Benchmarks.NonBurst {
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

        [BurstDiscard]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public Result Run() {
            return DoFibonacci(Number);
        }

        private static uint DoFibonacci(uint number) {
            if (number <= 1)
                return 1;

            return DoFibonacci(number - 1) + DoFibonacci(number - 2);
        }
    }
}
