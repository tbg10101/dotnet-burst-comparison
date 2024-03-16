using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace DotNetBurstComparison.Dotnet.Benchmarks;

/// <summary>
/// This is supposed to test vector multiplication.
/// https://docs.unity3d.com/Packages/com.unity.mathematics@1.3/manual/vector-multiplication.html
/// </summary>
[SimpleJob]
[IterationsColumn]
public class VectorMultiplication {
    private const int ArrayLength = 1_000_000;

    private readonly Vector4[] _vectorArrayA = new Vector4[ArrayLength];
    private readonly Vector4[] _vectorArrayB = new Vector4[ArrayLength];

    public VectorMultiplication() {
        for (int i = 0; i < ArrayLength; i++) {
            _vectorArrayA[i] = new Vector4(
                (float)Math.Sin(4 * i + 0),
                (float)Math.Sin(4 * i + 1),
                (float)Math.Sin(4 * i + 2),
                (float)Math.Sin(4 * i + 3)
            );
            _vectorArrayB[i] = new Vector4(
                (float)Math.Cos(4 * i + 0),
                (float)Math.Cos(4 * i + 1),
                (float)Math.Cos(4 * i + 2),
                (float)Math.Cos(4 * i + 3)
            );
        }
    }

    [Benchmark]
    public void Run() {
        Vector4[] a = _vectorArrayA;
        Vector4[] b = _vectorArrayB;

        for (int i = 0; i < ArrayLength; i++) {
            Vector4 vA = a[i];
            Vector4 vB = b[i];

            a[i] = vA * vB;
        }
    }
}
