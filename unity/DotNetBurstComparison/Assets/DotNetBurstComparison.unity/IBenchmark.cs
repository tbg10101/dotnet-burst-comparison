using System;

namespace DotNetBurstComparison.Unity {
    public interface IBenchmark: IDisposable {
        void RunNonBurst();
        void RunBurst();
    }
}
