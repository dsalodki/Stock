using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Stock.Jobs
{
    public class JobScheduler
    {
        public static async void Start(string connectionString)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<CheckExpirationJob>().WithIdentity("myJob", "CEJ").UsingJobData("connectionString", connectionString).Build();

            ITrigger trigger = TriggerBuilder.Create()

                .WithIdentity("myTrigger", "CEJ")

                .WithSimpleSchedule(x => x.WithIntervalInHours(24)
                    .RepeatForever())

                .StartAt(DateTimeOffset.UtcNow.AddMinutes(5))

                .WithPriority(1)

                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
