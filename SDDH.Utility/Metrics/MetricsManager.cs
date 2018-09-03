using SDDH.Metrics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SDDH.Utility.Metrics
{
    public class MetricsManager : IMetricsManager
    {
        public MetricsManager() { }

        private static readonly Lazy<MetricsManager> _lazyMetricsManager = new Lazy<MetricsManager>(() => new MetricsManager());

        public static IMetricsManager Instance
        {
            //get { return _lazyMetricsManager.Value; }
            get { return new MetricsManager(); }
        }


        #region Meter

        //private static MetricTags deafult_tags = new MetricTags(string.Format("AppId={0}", "100100"), string.Format("ServerIP={0}", "127.0.0.1"));
        private static MetricTags deafult_tags = new MetricTags();

        /// <summary>
        /// Metric数量统计
        /// </summary>
        /// <param name="key">MetricKey</param>
        /// <param name="unit">该数量的统计单位，默认‘次’</param>
        public void MeterMark(string key, string unit = "次")
        {
            try
            {
                Meter meter = JMetric.Meter(key, Unit.Custom(unit), TimeUnit.Seconds, deafult_tags);
                meter.Mark();
            }
            catch { }
        }

        public void MeterMark(string key, string item, string unit = "次")
        {
            try
            {
                Meter meter = JMetric.Meter(key, Unit.Custom(unit), TimeUnit.Seconds, deafult_tags);
                meter.Mark(item);
            }
            catch { }
        }

        public void MeterMark(string key, string item, string[] tagArr, string unit = "次")
        {
            try
            {
                MetricTags tags = deafult_tags;
                if (tagArr != null && tagArr.Length > 0)
                {
                    List<string> tagList = new List<string>(tags.Tags.Length + deafult_tags.Tags.Length);
                    tagList.AddRange(deafult_tags.Tags);
                    tagList.AddRange(tagArr);
                    tags = new MetricTags(tagList);
                }
                Meter meter = JMetric.Meter(key, Unit.Custom(unit), TimeUnit.Seconds, tags);

                if (string.IsNullOrWhiteSpace(item))
                {
                    meter.Mark();
                }
                else
                {
                    meter.Mark(item);
                }
            }
            catch { }
        }

        #endregion

        #region Histogram

        public void HistogramUpdate(string key, long value, string unit = "毫秒")
        {
            try
            {
                Histogram histogram = JMetric.Histogram(key, Unit.Custom(unit), SamplingType.FavourRecent, deafult_tags);
                histogram.Update(value);
            }
            catch { }
        }

        public void HistogramUpdate(string key, long value, string[] tagArr, string unit = "毫秒")
        {
            try
            {
                HistogramUpdate(key, "", value, tagArr, unit);
            }
            catch { }
        }

        public void HistogramUpdate(string key, string userValue, long value, string[] tagArr, string unit = "毫秒")
        {
            try
            {
                MetricTags tags = deafult_tags;
                if (tagArr != null && tagArr.Length > 0)
                {
                    List<string> tagList = new List<string>(tags.Tags.Length + deafult_tags.Tags.Length);
                    tagList.AddRange(deafult_tags.Tags);
                    tagList.AddRange(tagArr);
                    tags = new MetricTags(tagList);
                }
                Histogram histogram = JMetric.Histogram(key, Unit.Custom(unit), SamplingType.FavourRecent, tags);
                if (string.IsNullOrWhiteSpace(userValue))
                {
                    histogram.Update(value);
                }
                else
                {
                    histogram.Update(value, userValue);
                }
            }
            catch { }
        }

        #endregion


    }

}
