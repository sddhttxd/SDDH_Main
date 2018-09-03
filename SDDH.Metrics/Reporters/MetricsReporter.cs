using System;
using System.Threading;
using SDDH.Metrics.MetricData;

namespace SDDH.Metrics.Reporters
{
    public interface MetricsReporter : Utils.IHideObjectMembers
    {
        void RunReport(MetricsData metricsData, Func<HealthStatus> healthStatus, CancellationToken token);
    }
}
