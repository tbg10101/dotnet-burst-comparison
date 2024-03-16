using System;
using BenchmarkDotNet.Attributes;
using Unity.Burst;
using Unity.Jobs;

namespace DotNetBurstComparison.Unity.Benchmarks {
    /// <summary>
    /// Shamelessly borrowed: https://github.com/nxrighthere/BurstBenchmarks
    /// </summary>
    [SimpleJob]
    [IterationsColumn]
    public unsafe class SieveOfEratosthenes {
        private const uint Iterations = 1_000_000;

        [BurstDiscard]
        [Benchmark]
        [BenchmarkCategory("NonBurst")]
        public void RunNonBurst() {
            uint _ = DoSieveOfEratosthenes(Iterations);
        }

        [Benchmark]
        [BenchmarkCategory("Burst")]
        public void RunBurst() {
            SieveOfEratosthenesJob job = new() {
                Iterations = Iterations
            };

            job.Schedule().Complete();
        }

        [BurstCompile]
        private struct SieveOfEratosthenesJob : IJob {
            public uint Iterations;
            public uint Result;

            public void Execute() {
                Result = DoSieveOfEratosthenes(Iterations);
            }
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
}
