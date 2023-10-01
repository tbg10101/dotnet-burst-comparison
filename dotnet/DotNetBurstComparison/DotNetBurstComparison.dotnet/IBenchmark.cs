namespace DotNetBurstComparison.Dotnet;

public interface IBenchmark : IDisposable {
    void Run();
}
