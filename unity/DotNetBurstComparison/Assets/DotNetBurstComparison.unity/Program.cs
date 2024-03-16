using System;
using BenchmarkDotNet.Running;
using DotNetBurstComparison.Unity.Benchmarks;
using UnityEngine;

namespace DotNetBurstComparison.Unity {
    public static class Program {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Main() {
            BenchmarkSwitcher switcher = new(
                new[] {
                    typeof(LoopVectorization),
                    typeof(Fibonacci),
                    typeof(Mandelbrot),
                    typeof(SieveOfEratosthenes),
                    typeof(VectorMultiplication),
                    typeof(QuaternionMultiplication),
                    typeof(MatrixMultiplication),
                    typeof(Velocity),
                }
            );

            if (Application.isEditor) {
                switcher.RunAllJoined();
            } else {
                switcher.Run(Environment.GetCommandLineArgs());
            }
        }
    }
}
