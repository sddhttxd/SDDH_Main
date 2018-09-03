using Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace SDDH.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Metric.Config
                // Web监视仪表板，提供Metrics.NET度量值图表，浏览器打开这个地址可以访问一个Metrics.NET内置的页面
                .WithHttpEndpoint("http://localhost:8090/metrics/")
                //.WithAllCounters();
                // 配置报表输出
                .WithReporting((rc) =>
                {
                    // 报表输出到控制台
                    rc.WithConsoleReport(TimeSpan.FromSeconds(5));
                });
            //Console.ReadLine();
        }
    }
}
