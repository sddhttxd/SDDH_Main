﻿using System.Collections.Generic;
using System.Globalization;
using SDDH.Metrics.MetricData;
using SDDH.Metrics.Utils;

namespace SDDH.Metrics.Json
{
    public sealed class JsonBuilderV2
    {
        public const int Version = 2;
        public const string MetricsMimeType = "application/vnd.metrics.net.v2.metrics+json";

#if !DEBUG
        private const bool DefaultIndented = false;
#else
        private const bool DefaultIndented = true;
#endif
        public static string BuildJson(MetricsData data)
        {
            return BuildJson(data, AppEnvironment.Current, Clock.Default, indented: DefaultIndented);
        }

        public static string BuildJson(MetricsData data, IEnumerable<EnvironmentEntry> environment, Clock clock, bool indented = DefaultIndented)
        {
            var version = Version.ToString(CultureInfo.InvariantCulture);
            var timestamp = clock.UTCDateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff", CultureInfo.InvariantCulture);

            return JsonMetricsContext.FromContext(data, environment, version, timestamp)
                .ToJsonObject()
                .AsJson(indented);
        }
    }
}
