using Metrics;
using SDDH.Utility.Metrics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SDDH.Api.Controllers
{
    /// <summary>
    /// Metrics监控
    /// </summary>
    public class MetricsController : ApiController
    {
        private Counter _counter = Metric.Counter("public.counter", Unit.Custom("次"));
        private static Random ran = new Random(DateTime.Now.TimeOfDay.Milliseconds);

        /// <summary>
        /// Gauges
        /// </summary>
        [HttpGet]
        public void GaugeDemo(int number)
        {
            //Metric.Gauge("sddh.gauge", () => new Random().NextDouble() * 1000, Unit.None);
            Metric.Gauge("sddh.gauge", () => number, Unit.Custom("次"));
            _counter.Increment();
        }

        /// <summary>
        /// Counters
        /// </summary>
        [HttpGet]
        public void CounterDemo()
        {
            var counter = Metric.Counter("sddh.counter", Unit.Custom("并发"));

            Action doWork = () => { System.Threading.Thread.Sleep(ran.Next(10, 300)); };
            Action idlesse = () => { System.Threading.Thread.Sleep(ran.Next(0, 500)); };
            for (var i = 0; i < 20; i++)
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        counter.Increment();
                        doWork();
                        counter.Decrement();
                        idlesse();
                    }
                });
            }
            _counter.Increment();
        }

        /// <summary>
        /// Histograms
        /// </summary>
        [HttpGet]
        public void HistogramDemo()
        {
            var histogram = Metric.Histogram("sddh.histogram", Unit.Custom("岁"), SamplingType.LongTerm);

            Task.Run(() =>
            {
                while (true)
                {
                    histogram.Update(ran.Next(10, 80), ran.Next(0, 2) > 0 ? "男" : "女");
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            });
            _counter.Increment();
        }

        /// <summary>
        /// Meters
        /// </summary>
        [HttpGet]
        public void MeterDemo()
        {
            var meter = Metric.Meter("sddh.meter", Unit.Calls, TimeUnit.Seconds);

            Action idlesse = () => { System.Threading.Thread.Sleep(ran.Next(20, 50)); };
            Task.Run(() =>
            {
                while (true)
                {
                    meter.Mark();
                    idlesse();
                }
            });
            _counter.Increment();
        }

        /// <summary>
        /// Timers
        /// </summary>
        [HttpGet]
        public void TimerDemo()
        {
            var timer = Metric.Timer("sddh.meter", Unit.None, SamplingType.Default, TimeUnit.Seconds, TimeUnit.Microseconds);

            Action doWork = () => { System.Threading.Thread.Sleep(ran.Next(10, 300)); };
            Action idlesse = () => { System.Threading.Thread.Sleep(ran.Next(0, 500)); };
            for (var i = 0; i < 20; i++)
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        timer.Time(doWork);
                        idlesse();
                    }
                });
            }
            _counter.Increment();
        }

        /// <summary>
        /// HealthChecks
        /// </summary>
        [HttpGet]
        [HttpPost]
        public void HealthCheckDemo()
        {
            HealthChecks.RegisterHealthCheck("sddh.healthcheck", () =>
            {
                return ran.Next(100) < 5 ? HealthCheckResult.Unhealthy() : HealthCheckResult.Healthy();
            });
        }

        /// <summary>
        /// JMetricsDemo
        /// </summary>
        [HttpGet]
        [HttpPost]
        public void MetricsDemo(string serverIP = "172.17.0.120", string appId = "100120")
        {

            //var ms = new Random().Next(100, 30000);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            //int random = new Random().Next(1, 100) % 4;
            //switch (random)
            //{
            //    case 1:
            //        //Meter m1 = JMetric.Meter("TestKey", Unit.Custom("次"), TimeUnit.Seconds, new string[] { "serverIP=172.17.0.121", "appId=100121" });
            //        //m1.Mark("Test_Success");
            //        new MetricsManager().MeterMark("TestKey", "Test_Success", new string[] { "serverIP=172.17.0.121", "appId=100121" }, "次");
            //        //MetricsManager.Instance.HistogramUpdate("TestKey", ms, new string[] { "serverIP=172.17.0.121", "appId=100121" }, "毫秒");
            //        break;
            //    case 2:
            //        //Meter m2 = JMetric.Meter("TestKey", Unit.Custom("次"), TimeUnit.Seconds, new string[] { "serverIP=172.17.0.122", "appId=100122" });
            //        //m2.Mark("Test_Success");
            //        new MetricsManager().MeterMark("TestKey", "Test_Success", new string[] { "serverIP=172.17.0.122", "appId=100122" }, "次");
            //        //MetricsManager.Instance.HistogramUpdate("TestKey", ms, new string[] { "serverIP=172.17.0.122", "appId=100122" }, "毫秒");
            //        break;
            //    case 3:
            //        //Meter m3 = JMetric.Meter("TestKey", Unit.Custom("次"), TimeUnit.Seconds, new string[] { "serverIP=172.17.0.123", "appId=100123" });
            //        //m3.Mark("Test_Success");
            //        new MetricsManager().MeterMark("TestKey", "Test_Success", new string[] { "serverIP=172.17.0.123", "appId=100123" }, "次");
            //        //MetricsManager.Instance.HistogramUpdate("TestKey", ms, new string[] { "serverIP=172.17.0.123", "appId=100123" }, "毫秒");
            //        break;
            //    default:
            //        //Meter m = JMetric.Meter("TestKey", Unit.Custom("次"), TimeUnit.Seconds);
            //        //m.Mark("Test_Success");
            //        new MetricsManager().MeterMark("TestKey", "Test_Success", "次");
            //        //MetricsManager.Instance.HistogramUpdate("TestKey", ms, "毫秒");
            //        break;
            //}

            new MetricsManager().MeterMark("TestKey", "Test_Success", new string[] { string.Format("serverIP={0}", serverIP), string.Format("appId={0}", appId) }, "次");

            watch.Stop();
            var ms = watch.ElapsedMilliseconds;
            MetricsManager.Instance.HistogramUpdate("TestKey", ms, "毫秒");
        }
    }
}
