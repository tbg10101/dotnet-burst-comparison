using System;
using BenchmarkDotNet.Attributes;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

namespace DotNetBurstComparison.Unity.Benchmarks {
    /// <summary>
    /// Shamelessly borrowed: https://github.com/nxrighthere/BurstBenchmarks
    /// </summary>
    [SimpleJob]
    [IterationsColumn]
    public class Mandelbrot {
        private const uint Width = 1920;
        private const uint Height = 1080;
        private const uint Iterations = 8;

        [BurstDiscard]
        [Benchmark]
        [BenchmarkCategory("NonBurst")]
        public void RunNonBurst() {
            float _ = DoMandelbrot(Width, Height, Iterations);
        }

        [Benchmark]
        [BenchmarkCategory("Burst")]
        public void RunBurst() {
            MandelbrotJob job = new() {
                Width = Width,
                Height = Height,
                Iterations = Iterations
            };

            job.Schedule().Complete();
        }

        [BurstCompile]
        private struct MandelbrotJob : IJob {
            public uint Width;
            public uint Height;
            public uint Iterations;
            public float Result;

            public void Execute() {
                Result = DoMandelbrot(Width, Height, Iterations);
            }

            private static float DoMandelbrot(uint width, uint height, uint iterations) {
                const float left = -2.1f;
                const float right = 1.0f;
                const float top = -1.3f;
                const float bottom = 1.3f;

                float data = 0.0f;

                for (uint i = 0; i < iterations; i++) {
                    float deltaX = (right - left) / width;
                    float deltaY = (bottom - top) / height;
                    float coordinateX = left;

                    for (uint x = 0; x < width; x++) {
                        float coordinateY = top;

                        for (uint y = 0; y < height; y++) {
                            float workX = 0;
                            float workY = 0;
                            int counter = 0;

                            while (counter < 255 && math.sqrt((workX * workX) + (workY * workY)) < 2.0f) {
                                counter++;

                                float newX = (workX * workX) - (workY * workY) + coordinateX;

                                workY = 2 * workX * workY + coordinateY;
                                workX = newX;
                            }

                            data = workX + workY;
                            coordinateY += deltaY;
                        }

                        coordinateX += deltaX;
                    }
                }

                return data;
            }
        }

        private static float DoMandelbrot(uint width, uint height, uint iterations) {
            float data = 0.0f;

            for (uint i = 0; i < iterations; i++) {
                float
                    left = -2.1f,
                    right = 1.0f,
                    top = -1.3f,
                    bottom = 1.3f,
                    deltaX = (right - left) / width,
                    deltaY = (bottom - top) / height,
                    coordinateX = left;

                for (uint x = 0; x < width; x++) {
                    float coordinateY = top;

                    for (uint y = 0; y < height; y++) {
                        float workX = 0;
                        float workY = 0;
                        int counter = 0;

                        while (counter < 255 && Math.Sqrt((workX * workX) + (workY * workY)) < 2.0f) {
                            counter++;

                            float newX = (workX * workX) - (workY * workY) + coordinateX;

                            workY = 2 * workX * workY + coordinateY;
                            workX = newX;
                        }

                        data = workX + workY;
                        coordinateY += deltaY;
                    }

                    coordinateX += deltaX;
                }
            }

            return data;
        }
    }
}
