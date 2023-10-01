using System.Runtime.CompilerServices;

namespace DotNetBurstComparison.Dotnet.Benchmarks;

public sealed unsafe class SieveOfEratosthenes : IBenchmark {
    private const uint Iterations = 1_000_000; // 1_000_000

    public SieveOfEratosthenes() {
        // do nothing
    }

    public void Dispose() {
        // do nothing
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Run() {
        uint result = DoSieveOfEratosthenes(Iterations);
    }

    private static uint DoSieveOfEratosthenes(uint iterations) {
        const int size = 1024;

        byte* flags = stackalloc byte[size];
        uint a, b, c, prime, count = 0;

        for (a = 1; a <= iterations; a++) {
            count = 0;

            for (b = 0; b < size; b++) {
                flags[b] = 1; // True
            }

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
