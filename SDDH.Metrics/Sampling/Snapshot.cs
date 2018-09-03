﻿using System.Collections.Generic;
namespace SDDH.Metrics.Sampling
{
    public interface Snapshot
    {
        long Count { get; }
        double GetValue(double quantile);
        long Max { get; }
        string MaxUserValue { get; }
        double Mean { get; }
        double Median { get; }
        long Min { get; }
        string MinUserValue { get; }
        double Percentile75 { get; }
        double Percentile95 { get; }
        double Percentile98 { get; }
        double Percentile99 { get; }
        double Percentile999 { get; }
        int Size { get; }
        double StdDev { get; }
        IEnumerable<long> Values { get; }
        long Sum { get; }
    }
}
