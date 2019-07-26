using Quartz;
using SDDH.Utility.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Consoles
{
    public class JobDemo : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                Console.WriteLine(DateTime.Now);
                //LocalTxtLog.WriteLog("Test");
            });
        }
    }
}
