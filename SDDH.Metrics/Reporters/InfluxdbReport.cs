using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using SDDH.Metrics.Reporters;
using SDDH.Metrics.Utils;
using SDDH.Metrics.MetricData;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;

namespace SDDH.Metrics.Reporters
{
    public class InfluxdbReporter : MetricsReporter
    {
        private static MetricTags DEFAULT_TAGS = new MetricTags(string.Format("ServerIP={0}", GetLocalIP()));
        private CancellationToken token;
        private static readonly string[] GaugeColumns = new[] { "Value" };
        private static readonly string[] CounterColumns = new[] { "Count" };
        private static readonly string[] MeterColumns = new[] { "Total Count", "Unit Count", "Mean Rate", "1 Min Rate", "5 Min Rate", "15 Min Rate", };
        private static readonly string[] HistogramColumns = new[] 
        { 
            "Total Count", "Last", "Last User Value", "Min", "Min User Value", "Mean", "Max", "Max User Value",
            "StdDev", "Median", "Percentile 75%", "Percentile 95%", "Percentile 98%", "Percentile 99%", "Percentile 99.9%" , "Sample Size" };

        private static readonly string[] TimerColumns = new[] 
        {
            "Total Count", "Active Sessions",
            "Mean Rate", "1 Min Rate", "5 Min Rate", "15 Min Rate",
            "Last", "Last User Value", "Min", "Min User Value", "Mean", "Max", "Max User Value",
            "StdDev", "Median", "Percentile 75%", "Percentile 95%", "Percentile 98%", "Percentile 99%", "Percentile 99.9%" , "Sample Size" 
        };

        private readonly Uri influxdb;
        private readonly HttpClient httpClient;

        private List<InfluxRecord> data;

        protected string Context { get; private set; }
        protected DateTime Timestamp { get; private set; }

        public InfluxdbReporter(Uri influxdb, string username, string password)
        {
            this.influxdb = influxdb;

            var byteArray = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password));

            httpClient = new HttpClient()
            {
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray))
                }
            };
        }

        public void RunReport(MetricsData metricsData, Func<HealthStatus> healthStatus, CancellationToken token)
        {
            this.token = token;
            this.Timestamp = Clock.Default.UTCDateTime;

            var data = metricsData.OldFormat();

            this.Context = metricsData.Context;

            StartReport();
            ReportSection("Gauges", data.Gauges, g => ReportGauge(g));
            ReportSection("Counters", data.Counters, c => ReportCounter(c));
            ReportSection("Meters", data.Meters, m => ReportMeter(m));
            ReportSection("Histograms", data.Histograms, h => ReportHistogram(h));
            ReportSection("Timers", data.Timers, t => ReportTimer(t));
            ReportHealthStatus(healthStatus);
            EndReport();
        }


        private void StartReport()
        {
            this.data = new List<InfluxRecord>();
        }

        private void EndReport()
        {
            using (Metric.Context("Metrics.NET").Timer("influxdb.report", Unit.Calls).NewContext())
            {
                var content = string.Join("\n", data.Select(d => d.LineProtocol));
                var task = httpClient.PostAsync(influxdb, new StringContent(content));
                data = null;
                task.ContinueWith(m =>
                {
                    if (m.Result.IsSuccessStatusCode)
                    {
                        Metric.Context("Metrics.NET").Counter("influxdb.report.success", Unit.Events, DEFAULT_TAGS).Increment();
                    }
                    else
                    {
                        Metric.Context("Metrics.NET").Counter("influxdb.report.fail", Unit.Events, DEFAULT_TAGS).Increment();
                    }
                });
            }
        }

        private void ReportGauge(GaugeValueSource g)
        {
            string name = g.Name;
            double value = GetMetricValueSourceValue<double>(g.ValueProvider);
            Unit unit = g.Unit;

            if (!double.IsNaN(value) && !double.IsInfinity(value))
            {
                Pack(name, GaugeColumns, value, g.Tags);
            }
        }

        private void ReportCounter(CounterValueSource g)
        {
            string name = g.Name;
            CounterValue value = GetMetricValueSourceValue<CounterValue>(g.ValueProvider);
            Unit unit = g.Unit;

            var itemColumns = value.Items.SelectMany(i => new[] { i.Item + " - Count", i.Item + " - Percent" });
            var columns = CounterColumns.Concat(itemColumns);

            var itemValues = value.Items.SelectMany(i => new object[] { i.Count, i.Percent });
            var values = new object[] { value.Count }.Concat(itemValues);

            Pack(name, columns, values, g.Tags);
        }

        private void ReportMeter(MeterValueSource g)
        {
            string name = g.Name;
            MeterValue value = GetMetricValueSourceValue<MeterValue>(g.ValueProvider);
            Unit unit = g.Unit;
            TimeUnit rateUnit = g.RateUnit;

            var itemColumns = value.Items.SelectMany(i => new[]             
            { 
                i.Item + " - Count", 
                i.Item + " - Unit Count", 
                i.Item + " - Percent", 
                i.Item + " - Mean Rate",
                i.Item + " - 1 Min Rate",
                i.Item + " - 5 Min Rate",
                i.Item + " - 15 Min Rate"
            });
            var columns = MeterColumns.Concat(itemColumns);

            var itemValues = value.Items.SelectMany(i => new object[] 
            {
                i.Value.Count, 
                i.Value.UnitCount, 
                i.Percent,
                i.Value.MeanRate, 
                i.Value.OneMinuteRate, 
                i.Value.FiveMinuteRate, 
                i.Value.FifteenMinuteRate
            });

            var data = new object[] 
            {
                value.Count,
                value.UnitCount,
                value.MeanRate,
                value.OneMinuteRate,
                value.FiveMinuteRate,
                value.FifteenMinuteRate
            }.Concat(itemValues);

            Pack(name, columns, data, g.Tags);
        }

        private void ReportHistogram(HistogramValueSource g)
        {
            string name = g.Name;
            HistogramValue value = GetMetricValueSourceValue<HistogramValue>(g.ValueProvider);
            Unit unit = g.Unit;

            Pack(name, HistogramColumns, new object[]{
                value.Count,
                value.LastValue,
                value.LastUserValue ?? "\"\"",
                value.Min,
                value.MinUserValue ?? "\"\"",
                value.Mean,
                value.Max,
                value.MaxUserValue ?? "\"\"",
                value.StdDev,
                value.Median,
                value.Percentile75,
                value.Percentile95,
                value.Percentile98,
                value.Percentile99,
                value.Percentile999,
                value.SampleSize
            }, g.Tags);
        }

        private void ReportTimer(TimerValueSource g)
        {
            string name = g.Name;
            TimerValue value = GetMetricValueSourceValue<TimerValue>(g.ValueProvider);
            Unit unit = g.Unit;
            TimeUnit rateUnit = g.RateUnit;
            TimeUnit durationUnit = g.DurationUnit;

            Pack(name, TimerColumns, new object[]{
                value.Rate.Count,
                value.ActiveSessions,
                value.Rate.MeanRate,
                value.Rate.OneMinuteRate,
                value.Rate.FiveMinuteRate,
                value.Rate.FifteenMinuteRate,
                value.Histogram.LastValue,
                value.Histogram.LastUserValue ?? "\"\"",
                value.Histogram.Min,
                value.Histogram.MinUserValue ?? "\"\"",
                value.Histogram.Mean,
                value.Histogram.Max,
                value.Histogram.MaxUserValue ?? "\"\"",
                value.Histogram.StdDev,
                value.Histogram.Median,
                value.Histogram.Percentile75,
                value.Histogram.Percentile95,
                value.Histogram.Percentile98,
                value.Histogram.Percentile99,
                value.Histogram.Percentile999,
                value.Histogram.SampleSize
            }, g.Tags);
        }


        private void Pack(string name, IEnumerable<string> columns, object value, string[] tags)
        {
            Pack(name, columns, Enumerable.Repeat(value, 1), tags);
        }

        private void Pack(string name, IEnumerable<string> columns, IEnumerable<object> values, string[] tags)
        {
            this.data.Add(new InfluxRecord(name, Timestamp.ToUnixTime(), columns, values, tags));
        }

        private T GetMetricValueSourceValue<T>(MetricValueProvider<T> valueProvider)
        {
            return valueProvider.GetValue(false);
        }

        private void ReportHealth(HealthStatus status)
        {
        }

        private void StartMetricGroup(string metricName) { }
        private void EndMetricGroup(string metricName) { }

        private void ReportSection<T>(string name, IEnumerable<T> metrics, Action<T> reporter)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            if (metrics.Any())
            {
                StartMetricGroup(name);
                foreach (var metric in metrics)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    reporter(metric);
                }
                EndMetricGroup(name);
            }
        }

        private void ReportHealthStatus(Func<HealthStatus> healthStatus)
        {
            var status = healthStatus();
            if (!status.HasRegisteredChecks)
            {
                return;
            }
            StartMetricGroup("Health Checks");
            ReportHealth(status);
        }

        private static string GetLocalIP()
        {
            List<string> list = GetLocalIPList();
            if (list != null && list.Count > 0) return list[0];
            return string.Empty;
        }

        private static List<string> GetLocalIPList()
        {
            List<string> list = new List<string>();
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in ipHostEntry.AddressList)
            {

                if (Regex.IsMatch(ip.ToString(), @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$")) list.Add(ip.ToString());
            }
            return list;
        }

        private class InfluxRecord
        {
            public InfluxRecord(string name, long timestamp, IEnumerable<string> columns, IEnumerable<object> data, string[] tags)
            {
                var record = new StringBuilder();
                record.Append(Normalize(name));
                if (tags != null && tags.Length > 0)
                {
                    for (int i = 0; i < tags.Length; i++)
                    {
                        record.AppendFormat(",{0}", Normalize(tags[i]));
                    }
                }
                record.Append(" ");

                var fieldKeyPairs = new List<string>();
                foreach (var pair in columns.Zip(data, (col, dat) => new Tuple<string, object>(col, dat)))
                {
                    fieldKeyPairs.Add(string.Format("{0}={1}", Normalize(pair.Item1), pair.Item2));
                }

                record.Append(string.Join(",", fieldKeyPairs));
                record.Append(" ").Append(string.Format("{0:F0}", timestamp * 1e9));
                LineProtocol = record.ToString();
            }

            private static string Normalize(string v)
            {
                return v.Replace(" ", "\\ ");
            }

            public string LineProtocol { get; private set; }
        }
    }
}