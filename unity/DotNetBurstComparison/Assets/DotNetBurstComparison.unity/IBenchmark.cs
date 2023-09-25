using System;

namespace DotNetBurstComparison.unity {
    public interface IBenchmark: IDisposable {
        void RunNonBurst();
        void RunBurst();
    }
}
