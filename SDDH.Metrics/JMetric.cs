using SDDH.Metrics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDDH.Metrics
{
    public static class JMetric
    {
        static JMetric()
        {
            MetricsOptions options = OptionParser.ParserOptions();
            Metric.Config.WithReporting(config =>
            {
                if (options.Reporter == ReporterTypeEnum.Influxdb)
                {
                    config.WithInfluxDb(new Uri(options.ReportUrl), options.Username, options.Password, TimeSpan.FromSeconds(options.Interval));
                }
                else
                {
                    config.WithServiceReport(TimeSpan.FromMinutes(options.Interval), options.ReportUrl);
                }
            });

            string httpEndpoint = System.Configuration.ConfigurationManager.AppSettings["Metrics.HttpEndpoint"];
            if (!string.IsNullOrEmpty(httpEndpoint))
            {
                Metric.Config.WithHttpEndpoint(httpEndpoint);
            }
        }

        public static void Counter(string name, Func<double> valueProvider, Unit unit, MetricTags tags = default(MetricTags))
        {
            Metric.Gauge(name, valueProvider, unit, tags);
        }

        public static Counter Counter(string name, Unit unit, MetricTags tags = default(MetricTags))
        {
            return Metric.Counter(name, unit, tags);
        }

        public static Meter Meter(string name, Unit unit, TimeUnit rateUnit = TimeUnit.Seconds, MetricTags tags = default(MetricTags))
        {
            return Metric.Meter(name, unit, rateUnit, tags);
        }

        public static Histogram Histogram(string name, Unit unit, SamplingType samplingType = SamplingType.FavourRecent, MetricTags tags = default(MetricTags))
        {
            return Metric.Histogram(name, unit, samplingType, tags);
        }

        public static Timer Timer(string name,
            Unit unit, SamplingType samplingType = SamplingType.FavourRecent,
            TimeUnit rateUnit = TimeUnit.Seconds,
            TimeUnit durationUnit = TimeUnit.Milliseconds,
            MetricTags tags = default(MetricTags))
        {
            return Metric.Timer(name, unit, samplingType, rateUnit, durationUnit, tags);
        }
    }
}
