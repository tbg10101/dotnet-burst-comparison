using System.Runtime.CompilerServices;

namespace DotNetBurstComparison.Runner {

    public class EmptyBenchmark : IBenchmark {
        public EmptyBenchmark() {
            // do nothing
        }

        public void Dispose() {
            // do nothing
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public Result Run() {
            return default;
        }
    }
}
