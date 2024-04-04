using System;
using System.Collections.Generic;
using DotNetBurstComparison.Runner;

namespace DotNetBurstComparison.Common.Package {
    public class BenchmarkCollection {
        public enum BenchmarkTypes {
            LoopVectorization,
            Fibonacci,
            Mandelbrot,
            SieveOfEratosthenes,
            VectorMultiplication,
            QuaternionMultiplication,
            MatrixMultiplication,
            Velocity,
        }

        private static readonly Dictionary<BenchmarkTypes, uint> BenchmarkIterationCounts = new() {
            { BenchmarkTypes.LoopVectorization, 128 },
            { BenchmarkTypes.Fibonacci, 16 },
            { BenchmarkTypes.Mandelbrot, 16 },
            { BenchmarkTypes.SieveOfEratosthenes, 16 },
            { BenchmarkTypes.VectorMultiplication, 64 },
            { BenchmarkTypes.QuaternionMultiplication, 128 },
            { BenchmarkTypes.MatrixMultiplication, 128 },
            { BenchmarkTypes.Velocity, 128 },
        };

        private readonly Dictionary<BenchmarkTypes, BenchmarkConfiguration> _registeredBenchmarks = new();

        private readonly string _platform;

        public BenchmarkCollection(string platform) {
            _platform = platform;
        }

        public void RegisterBenchmark(BenchmarkTypes benchmarkType, Func<IBenchmark> benchmarkProvider) {
            _registeredBenchmarks[benchmarkType] = new BenchmarkConfiguration(
                $"{benchmarkType.ToString()} {_platform}",
                benchmarkProvider,
                BenchmarkIterationCounts[benchmarkType]);
        }

        public void Run(string outputPath) {
            foreach (BenchmarkTypes benchmarkType in Enum.GetValues(typeof(BenchmarkTypes))) {
                if (!_registeredBenchmarks.ContainsKey(benchmarkType)) {
                    throw new Exception($"Missing benchmark: {benchmarkType.ToString()}");
                }
            }

            IEnumerable<BenchmarkSummary> summaries = BenchmarkRunner.Run(_registeredBenchmarks.Values);

            summaries.WriteCsv(outputPath);
        }
    }
}
