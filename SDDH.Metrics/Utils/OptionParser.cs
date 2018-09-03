using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDDH.Metrics.Utils
{
    public class OptionParser
    {
        public static MetricsOptions ParserOptions()
        {
            MetricsOptions options = new MetricsOptions()
            {
                ReportUrl = "http://192.168.2.23:10001/Metrics.asmx",
                Interval = 1,
                Reporter = ReporterTypeEnum.Service
            };
            try
            {
                int configSeconds = 1;
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Metrics.ServiceReport.TimeSpan"], out configSeconds);
                string settingValue = System.Configuration.ConfigurationManager.AppSettings["Metrics.ServiceReportUrl"];
                if (!string.IsNullOrEmpty(settingValue))
                {
                    var settingArr = settingValue.Split(new char[] {  ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    options.ReportUrl = settingArr[0];
                    if (settingArr.Length < 2)
                    {
                        options.Interval = configSeconds == 0 ? 1 : configSeconds;
                        options.Reporter = ReporterTypeEnum.Service;
                        return options;
                    }
                    var optionsArr = settingArr[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < optionsArr.Length; i++)
                    {
                        string caseValue = optionsArr[i].Split('=')[0];
                        string optionValue = optionsArr[i].Split('=')[1];
                        switch (caseValue)
                        {
                            case "u":
                                options.Username = optionValue;
                                break;
                            case "p":
                                options.Password = optionValue;
                                break;
                            case "r":
                                options.Reporter = (ReporterTypeEnum)Enum.Parse(typeof(ReporterTypeEnum), optionValue, true);
                                break;
                            case "interval":
                                options.Interval = Convert.ToInt32(optionValue);
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (options.Interval <= 0)
                {
                    options.Interval = configSeconds == 0 ? 1 : configSeconds;
                }
                if (options.Reporter == ReporterTypeEnum.NN)
                {
                    options.Reporter = ReporterTypeEnum.Service;
                }
            }
            catch (Exception)
            {
            }
            return options;
        }
    }
}
