﻿using System.Numerics;
using System.Runtime.CompilerServices;

namespace DotNetBurstComparison.Dotnet.Benchmarks;

public sealed class Velocity : IBenchmark {
    private const int ArrayLength = 1_000_000; // 1_000_000
    private const float TimeDelta = 0.033f;

    private readonly Vector4[] _vectorArrayA = new Vector4[ArrayLength];
    private readonly Vector4[] _vectorArrayB = new Vector4[ArrayLength];

    public Velocity() {
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

    public void Dispose() {
        // do nothing
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Run() {
        Vector4[] a = _vectorArrayA;
        Vector4[] b = _vectorArrayB;

        for (int i = 0; i < ArrayLength; i++) {
            a[i] += TimeDelta * b[i];
        }
    }
}
