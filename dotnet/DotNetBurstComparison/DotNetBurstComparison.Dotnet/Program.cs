using System.Diagnostics;
using DotNetBurstComparison.Dotnet.Benchmarks;

namespace DotNetBurstComparison.Dotnet;

public static class Program {
    public static void Main() {
        PerformTests(new Func<IBenchmark>[] {
            () => new LoopVectorization(),
            () => new Fibonacci(),
            () => new Mandelbrot(),
            () => new SieveOfEratosthenes(),
            () => new VectorMultiplication(),
            () => new QuaternionMultiplication(),
            () => new MatrixMultiplication(),
            () => new Velocity(),
        });
    }

    private static void PerformTests(IReadOnlyList<Func<IBenchmark>> benchmarkingInitializers) {
        foreach (Func<IBenchmark> initializer in benchmarkingInitializers) {
            TestResult result = PerformTest(initializer);
            Console.WriteLine(result);
        }
    }

    private static TestResult PerformTest(Func<IBenchmark> benchmarkingInitializer) {
        IBenchmark benchmark = benchmarkingInitializer();

        string testName = benchmark.GetType().Name;
        Stopwatch sw = new();

        sw.Restart();
        benchmark.Run();
        sw.Stop();
        TimeSpan dotNetElapsedInitial = sw.Elapsed;

        sw.Restart();
        benchmark.Run();
        sw.Stop();
        TimeSpan dotNetElapsed = sw.Elapsed;

        benchmark.Dispose();

        return new TestResult(
            testName,
            dotNetElapsedInitial,
            dotNetElapsed);
    }
}

public readonly struct TestResult(
    string testName,
    TimeSpan elapsedTimeDotNetInitial,
    TimeSpan elapsedTimeDotNet
) {
    public readonly string TestName = testName;

    public readonly TimeSpan ElapsedTimeDotNetInitial = elapsedTimeDotNetInitial;
    public readonly TimeSpan ElapsedTimeDotNet = elapsedTimeDotNet;

    public override string ToString() {
        return $"{TestName}\n" +
               $"    .NET: {FormatTimeSpan(ElapsedTimeDotNetInitial)} | {FormatTimeSpan(ElapsedTimeDotNet)}";
    }

    private static string FormatTimeSpan(TimeSpan duration) {
        return duration.Ticks.ToString("N0");
    }
}
