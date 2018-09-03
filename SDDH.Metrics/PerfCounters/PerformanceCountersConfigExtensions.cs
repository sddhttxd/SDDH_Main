﻿
using SDDH.Metrics.PerfCounters;
namespace SDDH.Metrics
{
    public static class PerformanceCountersConfigExtensions
    {
        public const string DefaultMachineCountersContext = "System";
        public const string DefaultApplicationCountersContext = "Application";

        /// <summary>
        /// Register all pre-defined performance counters as Gauge metrics.
        /// This includes System Counters, CLR Global Counters and CLR App Counters.
        /// </summary>
        public static MetricsConfig WithAllCounters(this MetricsConfig config, string systemContext = DefaultMachineCountersContext,
            string applicationContext = DefaultApplicationCountersContext)
        {
            return config.WithSystemCounters(systemContext)
                .WithAppCounters(applicationContext);
        }

        /// <summary>
        /// Register all pre-defined system performance counters as Gauge metrics.
        /// This includes: Available RAM, CPU Usage, Disk Writes/sec, Disk Reads/sec
        /// </summary>
        public static MetricsConfig WithSystemCounters(this MetricsConfig config, string context = DefaultMachineCountersContext)
        {
            config.WithConfigExtension((ctx, hs) => PerformanceCounters.RegisterSystemCounters(ctx.Context(context)));
            return config;
        }

        /// <summary>
        /// Register application level, CLR related performance counters as Gauge metrics.
        /// This includes: Mb in all heaps, time in GC, exceptions per sec, Threads etc.
        /// </summary>
        public static MetricsConfig WithAppCounters(this MetricsConfig config, string context = DefaultApplicationCountersContext)
        {
            config.WithConfigExtension((ctx, hs) => PerformanceCounters.RegisterAppCounters(ctx.Context(context)));
            return config;
        }
    }
}
