using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DotNetBurstComparison.Runner {
    /// <summary>
    /// Roughly based on Benchmark.NET: https://benchmarkdotnet.org/articles/guides/how-it-works.html
    /// Which I would use but I need something consistent for Unity and .NET.
    /// </summary>
    public static class BenchmarkRunner {
        private const uint UnrollFactor = 16;

        public static IEnumerable<BenchmarkSummary> Run(IEnumerable<BenchmarkConfiguration> benchmarkConfigurations) {
            try {
                BenchmarkSummary[] results = benchmarkConfigurations
                    .Select(Run)
                    .ToArray();

                Console.WriteLine();
                Console.WriteLine("// Full Summary:");
                Console.WriteLine(string.Join('\n', results.Select(r => r.ToString())));
                Console.WriteLine();
                Console.Out.Flush();

                return results;
            } catch (Exception e) {
                Console.WriteLine($"// Encountered exception: {e.Message}\n{e.StackTrace}");
                throw;
            }
        }

        public static BenchmarkSummary Run(BenchmarkConfiguration benchmarkConfiguration) {
            if (benchmarkConfiguration.InvokeCount < UnrollFactor) {
                throw new ArgumentException(
                    $"Benchmark configuration invoke count must be greater than or equal to {UnrollFactor}.",
                    nameof(benchmarkConfiguration)
                );
            }

            Stopwatch totalTimeSw = new();
            totalTimeSw.Start();

            Console.WriteLine($"// Running: {benchmarkConfiguration.TestName}");
            Console.WriteLine();

            Console.WriteLine("// Initializing");
            EmptyBenchmark emptyBenchmark = new();
            IBenchmark actualBenchmark = benchmarkConfiguration.Initializer();
            Console.WriteLine();

            // warm up the empty benchmark
            Console.WriteLine("// Warming Overhead Benchmark");
            TimeSpan overheadWarmupMean = Run(emptyBenchmark, benchmarkConfiguration.InvokeCount, out _);
            Console.WriteLine($"Overhead Warmup Mean: {overheadWarmupMean.Formatted()}");
            Console.WriteLine();

            // run the empty benchmark
            Console.WriteLine("// Recording Overhead Benchmark");
            TimeSpan overheadMean = Run(emptyBenchmark, benchmarkConfiguration.InvokeCount, out _);
            Console.WriteLine($"Overhead Recorded Mean: {overheadMean.Formatted()}");
            Console.WriteLine();

            // warm up the actual benchmark
            Console.WriteLine("// Warming Actual Benchmark");
            TimeSpan actualWarmupMean = Run(actualBenchmark, benchmarkConfiguration.InvokeCount, out _);
            Console.WriteLine($"Actual Warmup Mean: {actualWarmupMean.Formatted()}");
            Console.WriteLine();

            // run and record the actual benchmark
            Console.WriteLine("// Recording Actual Benchmark");
            TimeSpan actualMean = Run(actualBenchmark, benchmarkConfiguration.InvokeCount, out uint actualInvokeCount);
            Console.WriteLine($"Actual Recorded Mean: {actualMean.Formatted()}");
            Console.WriteLine();

            // clean up
            Console.WriteLine("// Cleaning Up");
            emptyBenchmark.Dispose();
            actualBenchmark.Dispose();
            totalTimeSw.Stop();
            Console.WriteLine();

            // create summary
            BenchmarkSummary result = new(
                benchmarkConfiguration.TestName,
                actualMean - overheadMean,
                actualInvokeCount
            );

            Console.WriteLine("// Summary");
            Console.WriteLine($"Reporting Mean: {result.Mean.Formatted()}");
            Console.WriteLine($"Total Time: {totalTimeSw.Elapsed.Formatted()}");
            Console.WriteLine();
            Console.Out.Flush();

            return result;
        }

        private static TimeSpan Run(IBenchmark benchmark, uint invokeCount, out uint actualInvokeCount) {
            Stopwatch sw = new();

            for (actualInvokeCount = 0; actualInvokeCount < invokeCount; actualInvokeCount += UnrollFactor) {
                sw.Start();

                // run UnrollFactor times
                // the docs don't really make it clear why they do this
                benchmark.Run();
                benchmark.Run();
                benchmark.Run();
                benchmark.Run();
                benchmark.Run();
                benchmark.Run();
                benchmark.Run();
                benchmark.Run();
                benchmark.Run();
                benchmark.Run();
                benchmark.Run();
                benchmark.Run();
                benchmark.Run();
                benchmark.Run();
                benchmark.Run();
                benchmark.Run();

                sw.Stop();
            }

            // tabulate mean
            long averageTicks = sw.ElapsedTicks / actualInvokeCount;
            return TimeSpan.FromTicks(averageTicks);
        }
    }

    public class BenchmarkConfiguration {
        public readonly string TestName;
        public readonly Func<IBenchmark> Initializer;
        public readonly uint InvokeCount;

        public BenchmarkConfiguration(string testName, Func<IBenchmark> initializer, uint invokeCount) {
            TestName = testName;
            Initializer = initializer;
            InvokeCount = invokeCount;
        }
    }

    public class BenchmarkSummary {
        public readonly string TestName;
        public readonly TimeSpan Mean;
        public readonly uint ActualInvokeCount;

        public BenchmarkSummary(string testName, TimeSpan mean, uint actualInvokeCount) {
            TestName = testName;
            Mean = mean;
            ActualInvokeCount = actualInvokeCount;
        }

        public override string ToString() {
            return $"{TestName}:\n" +
                   $"    Mean: {Mean.Ticks}\n" +
                   $"    Actual Invocation Count: {ActualInvokeCount}";
        }
    }

    public static class Extensions {
        public static string Formatted(this TimeSpan duration) {
            return duration.Ticks.ToString("N0");
        }

        public static void WriteCsv(this IEnumerable<BenchmarkSummary> summaries, string path) {
            using (StreamWriter writer = File.CreateText(path)) {
                writer.WriteLine("Test Name,Mean (ticks),Actual Invocations");

                foreach (BenchmarkSummary summary in summaries) {
                    writer.WriteLine($"{summary.TestName},{summary.Mean.Ticks},{summary.ActualInvokeCount}");
                }
            }
        }
    }
}
