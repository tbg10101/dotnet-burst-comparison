using System;
using System.Runtime.InteropServices;

namespace DotNetBurstComparison.Runner {
    public interface IBenchmark : IDisposable {
        Result Run();
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Result {
        [FieldOffset(0)]
        public uint Uint;

        [FieldOffset(0)]
        public int Int;

        [FieldOffset(0)]
        public float Float;

        public static implicit operator Result(int v) => new() { Int = v };
        public static implicit operator Result(uint v) => new() { Uint = v };
        public static implicit operator Result(float v) => new() { Float = v };
    }
}
