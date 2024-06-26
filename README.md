# dotnet-burst-comparison

## What Is?

This is a collection of benchmarks implemented in various .NET environments: .NET, Unity's default runtime (Mono), IL2CPP, and Burst. In particular this focuses on loop vectorization and math. Three of the benchmarks (Fibonacci, SieveOfEratosthenes, Mandelbrot) are from [BurstBenchmarks](https://github.com/nxrighthere/BurstBenchmarks). All benchmarks use the recommended types for each platform (`UnityEngine.*` for non-Burst Unity, `Unity.Mathematics.*` for Burst, and `System.Numerics.*` for plain .NET) including the benchmarks ported from BurstBenchmarks. All benchmarks are single-threaded.

Each benchmark is run in phases:
* Overhead Warmup
* Overhead Measurement
* Benchmark Warmup
* Benchmark Measurement

The result reported is the average Benchmark Measurement minus the average Overhead Measurement.

This is meant to replicate the behavior of BenchmarkDotNet. BenchmarkDotNet is not used because it cannot be easily used with Unity. (as of the time of writing)

## How To?

These instructions are written assuming you are running Windows on an x86_64 CPU.

All commands are executed from the repository root.

IMPORTANT: ensure that your runtime envrironment is as quiet as possible. This means closing all applications and asx many background tasks as possible. I also disconnect my computer from the Internet while running these to prevent network-initialted work from starting. I have also found that certain tasks will start when the user is deemed to be idle. I used a program to move my cursor every 15 seconds to prevent such tasks from starting.

### Unity Project

Download the version of unity specified in `unity\DotNetBurstComparison\ProjectSettings\ProjectVersion.txt`. Don't forget to also install the IL2CPP module.

Build the Unity project by opening the Unity project and use the Build > Windows menu item or this command:

```
"C:\Program Files\Unity\Hub\Editor\2023.2.19f1\Editor\Unity.exe" -batchmode -nographics -quit -projectPath unity\DotNetBurstComparison -executeMethod Editor.Build.BuildWindows -logfile unity\DotNetBurstComparison\Builds\Windows\log.txt
```

To run the Mono Unity program:

```
.\unity\DotNetBurstComparison\Builds\Windows\Mono\UnityBench.exe -batchmode -nographics -logfile unity\DotNetBurstComparison\Builds\Windows\Mono\log.txt
```

To run the IL2CPP Unity program:

```
.\unity\DotNetBurstComparison\Builds\Windows\IL2CPP\UnityBench.exe -batchmode -nographics -logfile unity\DotNetBurstComparison\Builds\Windows\IL2CPP\log.txt
```

The results will be in the log files in the commands above. Sadly Unity does not write logs to the command-line on Windows. (at the time of writing)

### .NET Project

Build the .NET project with this command:

```
dotnet build .\dotnet\DotNetBurstComparison\DotNetBurstComparison.sln -c Release
```

To run the .NET program:

```
dotnet dotnet\DotNetBurstComparison\DotNetBurstComparison.Dotnet\bin\Release\net8.0\DotNetBurstComparison.Dotnet.dll
```

The results will be printed to standard out.

## Results

Smaller values are better in the graphs below.

![LoopVectorization](images/LoopVectorization.png)

![VectorMultiplication](images/VectorMultiplication.png)

![QuaternionMultiplication](images/QuaternionMultiplication.png)

![MatrixMultiplication](images/MatrixMultiplication.png)

![Velocity](images/Velocity.png)

![Fibonacci](images/Fibonacci.png)

![SieveOfEratosthenes](images/SieveOfEratosthenes.png)

![Mandelbrot](images/Mandelbrot.png)

The computer running these tests:
* CPU: Intel Core i9 9900K @ 5GHz (all cores)
* Memory: 32GB DDR4 @ 3200MHz
* OS (from `dxdiag`): Windows 10 Pro 64-bit (10.0, Build 19045)
* .NET (`dotnet --version`): 8.0.204

### Conclusions

TLDR for SIMD workloads: IL2CPP+Burst > .NET8 == Mono+Burst > IL2CPP > Mono

Surprisingly .NET 8 can be competitive with Mono+Burst, except in SieveOfEratosthenes for some reason. (not sure why)

IL2CPP+Burst is crazy fast. If you don't need modding support for your Unity game and you are using Mono+Burst it seems like a no-brainer to switch to IL2CPP+Burst. (as long as you can support the more complicated build environment)

Unity's default Mono (without Burst) runtime is particularly atrocious. It would be wonderful to see Unity update to .NET 8 but I understand that it is mre complicated than just flipping a switch. And they have already announced they are moving in that direction.
