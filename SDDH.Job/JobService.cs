using Quartz;
using Quartz.Impl;
using SDDH.Utility.Log;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Job
{
    public partial class JobService : ServiceBase
    {
        public JobService()
        {
            InitializeComponent();
        }

        protected override async void OnStart(string[] args)
        {
            LocalTxtLog.WriteLog("Service Start!");
            await RunDemo();
        }

        protected override void OnStop()
        {
            LocalTxtLog.WriteLog("Service Stop!");
        }


        private static async Task RunDemo()
        {
            //1.通过工厂获取一个调度器的实例
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();

            //2.创建任务对象
            IJobDetail job = JobBuilder.Create<DemoJob>()
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
