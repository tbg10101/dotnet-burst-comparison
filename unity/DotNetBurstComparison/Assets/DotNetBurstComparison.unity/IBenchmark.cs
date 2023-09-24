using System;

namespace DotNetBurstComparison.unity {
    public interface IBenchmark: IDisposable {
        void RunBurst();
        void RunNonBurst();
    }
}
