using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Metrics
{
    public interface IMetricsManager
    {
        void MeterMark(string key, string unit);

        void MeterMark(string key, string item, string unit);

        void MeterMark(string key, string item, string[] tagArr, string unit);

        void HistogramUpdate(string key, long value, string unit);

        void HistogramUpdate(string key, long value, string[] tagArr, string unit);

        void HistogramUpdate(string key, string userValue, long value, string[] tagArr, string unit);
    }
}
