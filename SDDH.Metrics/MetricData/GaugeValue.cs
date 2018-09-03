﻿
namespace SDDH.Metrics.MetricData
{
    /// <summary>
    /// Combines the value of a gauge (a double) with the defined unit for the value.
    /// </summary>
    public sealed class GaugeValueSource : MetricValueSource<double>
    {
        public GaugeValueSource(string name, MetricValueProvider<double> value, Unit unit, MetricTags tags)
            : base(name, value, unit, tags)
        { }
    }
}
