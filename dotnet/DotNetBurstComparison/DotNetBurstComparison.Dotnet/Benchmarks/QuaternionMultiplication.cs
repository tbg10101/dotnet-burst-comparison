using System.Numerics;
using System.Runtime.CompilerServices;

namespace DotNetBurstComparison.Dotnet.Benchmarks;

/// <summary>
/// This is supposed to test quaternion multiplication.
/// https://docs.unity3d.com/Packages/com.unity.mathematics@1.3/manual/quaternion-multiplication.html
/// </summary>
public sealed class QuaternionMultiplication: IBenchmark {
    private const int ArrayLength = 1_000_000; // 1_000_000

    private readonly Quaternion[] _quaternionArrayA = new Quaternion[ArrayLength];
    private readonly Quaternion[] _quaternionArrayB = new Quaternion[ArrayLength];

    public QuaternionMultiplication() {
        Vector3 axis = new(0.0f, 1.0f, 0.0f);

        for (int i = 0; i < ArrayLength; i++) {
            _quaternionArrayA[i] = Quaternion.CreateFromAxisAngle(axis, (float)(i % 2.0f * Math.PI));
            _quaternionArrayB[i] = Quaternion.CreateFromAxisAngle(axis, (float)((i + 0.125f * Math.PI) % 2.0f * Math.PI));
        }
    }

    public void Dispose() {
        // do nothing
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Run() {
        Quaternion[] a = _quaternionArrayA;
        Quaternion[] b = _quaternionArrayB;

        for (int i = 0; i < ArrayLength; i++) {
            a[i] *= b[i];
        }
    }
}
