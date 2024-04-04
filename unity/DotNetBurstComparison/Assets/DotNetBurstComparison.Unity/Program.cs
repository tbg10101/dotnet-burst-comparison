using System;
using System.IO;
using System.Text;
using DotNetBurstComparison.Common.Package;
using UnityEngine;

namespace DotNetBurstComparison.Unity {
    public static class Program {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Main() {
            const string runtime =
#if ENABLE_MONO
                "Mono"
#elif ENABLE_IL2CPP
                "IL2CPP"
#endif
            ;

            // redirect Console.out to Unity logs
            Console.SetOut(new UnityLogWriter());

            // NON-BURST BENCHMARKS
            BenchmarkCollection nonBurstBenchmarkCollection = new(runtime);

            nonBurstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.LoopVectorization,
                () => new Benchmarks.NonBurst.LoopVectorization());
            nonBurstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.Fibonacci,
                () => new Benchmarks.NonBurst.Fibonacci());
            nonBurstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.Mandelbrot,
                () => new Benchmarks.NonBurst.Mandelbrot());
            nonBurstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.SieveOfEratosthenes,
                () => new Benchmarks.NonBurst.SieveOfEratosthenes());
            nonBurstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.VectorMultiplication,
                () => new Benchmarks.NonBurst.VectorMultiplication());
            nonBurstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.QuaternionMultiplication,
                () => new Benchmarks.NonBurst.QuaternionMultiplication());
            nonBurstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.MatrixMultiplication,
                () => new Benchmarks.NonBurst.MatrixMultiplication());
            nonBurstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.Velocity,
                () => new Benchmarks.NonBurst.Velocity());

            nonBurstBenchmarkCollection.Run($"{runtime}-results.csv");

            // BURST BENCHMARKS
            BenchmarkCollection burstBenchmarkCollection = new($"{runtime}-Burst");

            burstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.LoopVectorization,
                () => new Benchmarks.Burst.LoopVectorization());
            burstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.Fibonacci,
                () => new Benchmarks.Burst.Fibonacci());
            burstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.Mandelbrot,
                () => new Benchmarks.Burst.Mandelbrot());
            burstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.SieveOfEratosthenes,
                () => new Benchmarks.Burst.SieveOfEratosthenes());
            burstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.VectorMultiplication,
                () => new Benchmarks.Burst.VectorMultiplication());
            burstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.QuaternionMultiplication,
                () => new Benchmarks.Burst.QuaternionMultiplication());
            burstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.MatrixMultiplication,
                () => new Benchmarks.Burst.MatrixMultiplication());
            burstBenchmarkCollection.RegisterBenchmark(
                BenchmarkCollection.BenchmarkTypes.Velocity,
                () => new Benchmarks.Burst.Velocity());

            burstBenchmarkCollection.Run($"{runtime}-Burst-results.csv");
        }
    }

    internal class UnityLogWriter : TextWriter {
        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine() {
            Debug.Log("");
        }

        public override void WriteLine(string value) {
            Debug.Log($"{value}");
        }
    }
}
