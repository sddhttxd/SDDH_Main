﻿
namespace SDDH.Metrics.Json
{
    public class JsonMetric
    {
        private string[] tags = MetricTags.None.Tags;

        public string Name { get; set; }
        public string Unit { get; set; }
        public string[] Tags { get { return this.tags; } set { this.tags = value ?? new string[0]; } }
    }
}
