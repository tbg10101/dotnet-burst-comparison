using BenchmarkDotNet.Attributes;

namespace DotNetBurstComparison.Dotnet.Benchmarks;

/// <summary>
/// Shamelessly borrowed: https://github.com/nxrighthere/BurstBenchmarks
/// </summary>
[SimpleJob]
[IterationsColumn]
public class Fibonacci {
    private const uint Number = 46;

    [Benchmark]
    public void Run() {
        uint _ = DoFibonacci(Number);
    }

    private static uint DoFibonacci(uint number) {
        if (number <= 1)
            return 1;

        return DoFibonacci(number - 1) + DoFibonacci(number - 2);
    }
}
