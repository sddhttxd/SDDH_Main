using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDDH.Metrics
{
    public class MetricsOptions
    {
        public string ReportUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ReporterTypeEnum Reporter { get; set; }
        public int Interval { get; set; }
    }

    public enum ReporterTypeEnum
    {
        NN,
        Service,
        Influxdb
    }
}
