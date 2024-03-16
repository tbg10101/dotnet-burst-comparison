using BenchmarkDotNet.Running;
using DotNetBurstComparison.Dotnet.Benchmarks;

namespace DotNetBurstComparison.Dotnet;

public static class Program {
    public static void Main(string[] args) {
        BenchmarkSwitcher switcher = new(
            [
                typeof(LoopVectorization),
                typeof(Fibonacci),
                typeof(Mandelbrot),
                typeof(SieveOfEratosthenes),
                typeof(VectorMultiplication),
                typeof(QuaternionMultiplication),
                typeof(MatrixMultiplication),
                typeof(Velocity),
            ]
        );
        switcher.Run(args);
    }
}
