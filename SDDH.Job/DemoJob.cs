using Quartz;
using SDDH.Utility.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Job
{
    public class DemoJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                LocalTxtLog.WriteLog("Test");
            });
        }
    }
}
