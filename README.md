# dotnet-burst-comparison

Benchmarks to compare .NET and and Unity's Burst compiler.

# What Is?

This is a collection of benchmarks implementated in various .NET environments: .NET, Unity's default runtime, IL2CPP, and Burst.

# How To?

These instructions are written assuming you are running Windows on an x86_64 CPU.

All commands are executed from the repository root.

### Unity Project

Build the Unity project by opening the Unity project and use the Build > Windows menu item or this command:

```
'C:\Program Files\Unity\Hub\Editor\2023.1.13f1\Editor\Unity.exe' -batchmode -nographics -quit -projectPath unity\DotNetBurstComparison -executeMethod Editor.Build.BuildWindows
```

To run the Mono Unity program:

```
.\unity\DotNetBurstComparison\Builds\Windows\Mono\UnityBench.exe -batchmode -nographics -logfile .\unity\DotNetBurstComparison\Builds\Windows\Mono\log.txt
```

To run the IL2CPP Unity program:

```
.\unity\DotNetBurstComparison\Builds\Windows\IL2CPP\UnityBench.exe -batchmode -nographics -logfile .\unity\DotNetBurstComparison\Builds\Windows\IL2CPP\log.txt
```

### .NET Project

Build the .NET project with this command:

```
dotnet build .\dotnet\DotNetBurstComparison\DotNetBurstComparison.sln -c Release
```

To run the .NET program:

```
dotnet dotnet\DotNetBurstComparison\DotNetBurstComparison.Dotnet\bin\Release\net8.0\DotNetBurstComparison.Dotnet.dll
```
