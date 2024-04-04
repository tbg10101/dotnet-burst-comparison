using System;
using System.Runtime.CompilerServices;
using DotNetBurstComparison.Runner;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace DotNetBurstComparison.Unity.Benchmarks.Burst {
    public sealed class SieveOfEratosthenes : IBenchmark {
        private const uint Iterations = 1_000_000;

        public SieveOfEratosthenes() {
            // do nothing
        }

        public void Dispose() {
            // do nothing
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public Result Run() {
            SieveOfEratosthenesJob job = new() {
                Iterations = Iterations
            };

            job.Schedule().Complete();

            return job.Result;
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

            // other implementations use a stackalloc byte array but using that approach with Burst causes the program to hang
            // Span<byte> flags = stackalloc byte[size];

            NativeArray<byte> flagsArray = new(size, Allocator.Temp);
            Span<byte> flags = flagsArray.AsSpan();
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

            flagsArray.Dispose();

            return count;
        }
    }
}
