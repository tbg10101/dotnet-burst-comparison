using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace DotNetBurstComparison.Dotnet.Benchmarks;

/// <summary>
/// This is supposed to test matrix multiplication.
/// https://docs.unity3d.com/Packages/com.unity.mathematics@1.3/manual/4x4-matrices.html
/// </summary>
[SimpleJob]
[IterationsColumn]
public class MatrixMultiplication {
    private const int ArrayLength = 1_000_000;

    private readonly Matrix4x4[] _vectorArrayA = new Matrix4x4[ArrayLength];
    private readonly Matrix4x4[] _vectorArrayB = new Matrix4x4[ArrayLength];

    public MatrixMultiplication() {
        Vector3 axis = new(0.0f, 1.0f, 0.0f);

        for (int i = 0; i < ArrayLength; i++) {
            _vectorArrayA[i] = CreateFromTranslationRotationScale(
                new Vector3((float)Math.Sin(3 * i + 0), (float)Math.Sin(3 * i + 1), (float)Math.Sin(3 * i + 2)),
                Quaternion.CreateFromAxisAngle(axis, (float)(i % 2.0f * Math.PI)),
                new Vector3(1.0f, 1.0f, 1.0f));

            _vectorArrayB[i] = CreateFromTranslationRotationScale(
                new Vector3((float)Math.Cos(3 * i + 0), (float)Math.Cos(3 * i + 1), (float)Math.Cos(3 * i + 2)),
                Quaternion.CreateFromAxisAngle(axis, (float)(i % 2.0f * Math.PI)),
                new Vector3(1.0f, 1.0f, 1.0f));
        }
    }

    [Benchmark]
    public void Run() {
        Matrix4x4[] a = _vectorArrayA;
        Matrix4x4[] b = _vectorArrayB;

        for (int i = 0; i < ArrayLength; i++) {
            a[i] *= b[i];
        }
    }

    private static Matrix4x4 CreateFromTranslationRotationScale(Vector3 translation, Quaternion rotation, Vector3 scale) {
        Matrix4x4 m = Matrix4x4.Transform(Matrix4x4.CreateScale(scale), rotation);
        m.Translation = translation;
        return m;
    }
}
