using System.Runtime.CompilerServices;

namespace DotNetBurstComparison.Dotnet.Benchmarks;

/// <summary>
/// Shamelessly borrowed: https://github.com/nxrighthere/BurstBenchmarks
/// </summary>
public sealed class Fibonacci : IBenchmark {
    private const uint Number = 46; // 46

    public Fibonacci() {
        // do nothing
    }

    public void Dispose() {
        // do nothing
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Run() {
        uint result = DoFibonacci(Number);
    }

    private static uint DoFibonacci(uint number) {
        if (number <= 1)
            return 1;

        return DoFibonacci(number - 1) + DoFibonacci(number - 2);
    }
}
