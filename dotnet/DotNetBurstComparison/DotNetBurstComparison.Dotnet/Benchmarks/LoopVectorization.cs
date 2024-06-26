using System.Runtime.CompilerServices;
using DotNetBurstComparison.Runner;

namespace DotNetBurstComparison.Dotnet.Benchmarks;

/// <summary>
/// This is supposed to test loop vectorization.
/// https://docs.unity3d.com/Packages/com.unity.burst@1.8/manual/optimization-loop-vectorization.html
/// </summary>
public sealed class LoopVectorization : IBenchmark {
    private const int ArrayLength = 1_000_000;

    private readonly float[] _floatArrayA = new float[ArrayLength];
    private readonly float[] _floatArrayB = new float[ArrayLength];

    public LoopVectorization() {
        for (int i = 0; i < ArrayLength; i++) {
            _floatArrayA[i] = (float)Math.Sin(i);
            _floatArrayB[i] = (float)Math.Cos(i);
        }
    }

    public void Dispose() {
        // do nothing
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public Result Run() {
        float[] a = _floatArrayA;
        float[] b = _floatArrayB;

        for (int i = 0; i < ArrayLength; i++) {
            a[i] += b[i];
        }

        return default;
    }
}
