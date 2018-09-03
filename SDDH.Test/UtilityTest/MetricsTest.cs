using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDDH.Metrics;
using SDDH.Utility.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDDH.Test.UtilityTest
{
    [TestClass]
    public class MetricsTest
    {
        [TestMethod]
        public void MeterTest()
        {
            new Task(() =>
            {
                while (true)
                {
                    var ms = new Random().Next(100, 30000);
                    int random = new Random().Next(1, 100) % 4;
                    switch (random)
                    {
                        case 1:
                            //Meter m1 = JMetric.Meter("TestKey", Unit.Custom("次"), TimeUnit.Seconds, new string[] { "serverIP=172.17.0.121", "appId=100121" });
                            //m1.Mark("Test_Success");
                            new MetricsManager().MeterMark("TestKey", "Test_Success", new string[] { "serverIP=172.17.0.121", "appId=100121" }, "次");
                            //MetricsManager.Instance.HistogramUpdate("TestKey", ms, new string[] { "serverIP=172.17.0.121", "appId=100121" }, "毫秒");
                            break;
                        case 2:
                            //Meter m2 = JMetric.Meter("TestKey", Unit.Custom("次"), TimeUnit.Seconds, new string[] { "serverIP=172.17.0.122", "appId=100122" });
                            //m2.Mark("Test_Success");
                            new MetricsManager().MeterMark("TestKey", "Test_Success", new string[] { "serverIP=172.17.0.122", "appId=100122" }, "次");
                            //MetricsManager.Instance.HistogramUpdate("TestKey", ms, new string[] { "serverIP=172.17.0.122", "appId=100122" }, "毫秒");
                            break;
                        case 3:
                            //Meter m3 = JMetric.Meter("TestKey", Unit.Custom("次"), TimeUnit.Seconds, new string[] { "serverIP=172.17.0.123", "appId=100123" });
                            //m3.Mark("Test_Success");
                            new MetricsManager().MeterMark("TestKey", "Test_Success", new string[] { "serverIP=172.17.0.123", "appId=100123" }, "次");
                            //MetricsManager.Instance.HistogramUpdate("TestKey", ms, new string[] { "serverIP=172.17.0.123", "appId=100123" }, "毫秒");
                            break;
                        default:
                            //Meter m = JMetric.Meter("TestKey", Unit.Custom("次"), TimeUnit.Seconds);
                            //m.Mark("Test_Success");
                            new MetricsManager().MeterMark("TestKey", "Test_Success", "次");
                            //MetricsManager.Instance.HistogramUpdate("TestKey", ms, "毫秒");
                            break;
                    }
                    MetricsManager.Instance.HistogramUpdate("TestKey", ms, "毫秒");
                    Thread.Sleep(ms);
                }
            }).Start();
            Console.ReadKey();
        }


    }




}
