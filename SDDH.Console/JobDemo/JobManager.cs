using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Consoles
{
    public class JobManager
    {
        public static async Task RunDemo()
        {
            //1.通过工厂获取一个调度器的实例
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();

            //2.创建任务对象
            IJobDetail job = JobBuilder.Create<JobDemo>()
                .WithIdentity("JobName", "Group")
                .WithDescription("DemoJob")
                .Build();

            //3.创建触发器
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("TriggerName", "Group")
                .WithIdentity("DemoTrigger")
                .StartNow()
                .WithCronSchedule("0/10 * * * * ?")
                .Build();


            await scheduler.ScheduleJob(job, trigger);

        }

    }
}
