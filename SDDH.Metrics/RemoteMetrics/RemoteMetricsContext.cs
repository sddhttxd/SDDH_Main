﻿using System;
using System.Threading;
using System.Threading.Tasks;
using SDDH.Metrics.Core;
using SDDH.Metrics.Json;
using SDDH.Metrics.MetricData;
using SDDH.Metrics.Utils;

namespace SDDH.Metrics.RemoteMetrics
{
    public sealed class RemoteMetricsContext : ReadOnlyMetricsContext, MetricsDataProvider
    {
        private readonly Scheduler scheduler;

        private MetricsData currentData = MetricsData.Empty;

        public RemoteMetricsContext(Uri remoteUri, TimeSpan updateInterval, Func<string, JsonMetricsContext> deserializer)
            : this(new ActionScheduler(), remoteUri, updateInterval, deserializer)
        { }

        public RemoteMetricsContext(Scheduler scheduler, Uri remoteUri, TimeSpan updateInterval, Func<string, JsonMetricsContext> deserializer)
        {
            this.scheduler = scheduler;
            this.scheduler.Start(updateInterval, (c) => UpdateMetrics(remoteUri, deserializer, c));
        }

        private void UpdateMetrics(Uri remoteUri, Func<string, JsonMetricsContext> deserializer, CancellationToken token)
        {
            try
            {
                var remoteContext =  Metrics.Visualization.TaskHelper.RunAsync<JsonMetricsContext>(() => { return HttpRemoteMetrics.FetchRemoteMetrics(remoteUri, deserializer, token); }, null).Result;
                remoteContext.Environment.Add("RemoteUri", remoteUri.ToString());
                remoteContext.Environment.Add("RemoteVersion", remoteContext.Version);
                remoteContext.Environment.Add("RemoteTimestamp", remoteContext.Timestamp);

                this.currentData = remoteContext.ToMetricsData();
            }
            catch (Exception x)
            {
                MetricsErrorHandler.Handle(x, "Error updating metrics data from " + remoteUri.ToString());
                this.currentData = MetricsData.Empty;
            }
        }

        public override MetricsDataProvider DataProvider { get { return this; } }
        public MetricsData CurrentMetricsData { get { return this.currentData; } }

        public override void Dispose(bool disposing)
        {
            using (this.scheduler) { }
            base.Dispose(disposing);
        }
    }
}
