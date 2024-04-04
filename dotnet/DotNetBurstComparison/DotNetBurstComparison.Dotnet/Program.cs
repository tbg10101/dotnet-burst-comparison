using DotNetBurstComparison.Common.Package;
using DotNetBurstComparison.Dotnet.Benchmarks;

namespace DotNetBurstComparison.Dotnet;

public static class Program {
    public static void Main() {
        BenchmarkCollection benchmarkCollection = new(".NET8");

        benchmarkCollection.RegisterBenchmark(
            BenchmarkCollection.BenchmarkTypes.LoopVectorization,
            () => new LoopVectorization());
        benchmarkCollection.RegisterBenchmark(
            BenchmarkCollection.BenchmarkTypes.Fibonacci,
            () => new Fibonacci());
        benchmarkCollection.RegisterBenchmark(
            BenchmarkCollection.BenchmarkTypes.Mandelbrot,
            () => new Mandelbrot());
        benchmarkCollection.RegisterBenchmark(
            BenchmarkCollection.BenchmarkTypes.SieveOfEratosthenes,
            () => new SieveOfEratosthenes());
        benchmarkCollection.RegisterBenchmark(
            BenchmarkCollection.BenchmarkTypes.VectorMultiplication,
            () => new VectorMultiplication());
        benchmarkCollection.RegisterBenchmark(
            BenchmarkCollection.BenchmarkTypes.QuaternionMultiplication,
            () => new QuaternionMultiplication());
        benchmarkCollection.RegisterBenchmark(
            BenchmarkCollection.BenchmarkTypes.MatrixMultiplication,
            () => new MatrixMultiplication());
        benchmarkCollection.RegisterBenchmark(
            BenchmarkCollection.BenchmarkTypes.Velocity,
            () => new Velocity());

        benchmarkCollection.Run("dotnet8-results.csv");
    }
}
