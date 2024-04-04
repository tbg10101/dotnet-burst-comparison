using System.Runtime.CompilerServices;
using DotNetBurstComparison.Runner;

namespace DotNetBurstComparison.Dotnet.Benchmarks;

/// <summary>
/// Shamelessly borrowed: https://github.com/nxrighthere/BurstBenchmarks
/// </summary>
public sealed unsafe class SieveOfEratosthenes : IBenchmark {
    private const uint Iterations = 1_000_000;

    public SieveOfEratosthenes() {
        // do nothing
    }

    public void Dispose() {
        // do nothing
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public Result Run() {
        return DoSieveOfEratosthenes(Iterations);
    }

    private static uint DoSieveOfEratosthenes(uint iterations) {
        const int size = 1024;

        Span<byte> flags = stackalloc byte[size];
        uint count = 0;
        int a, b, c, prime;

        for (a = 1; a <= iterations; a++) {
            count = 0;

            flags.Fill(1); // True

            for (b = 0; b < size; b++) {
                if (flags[b] == 1) {
                    prime = b + b + 3;
                    c = b + prime;

                    while (c < size) {
                        flags[c] = 0; // False
                        c += prime;
                    }

                    count++;
                }
            }
        }

        return count;
    }
}
