using SwapControl.MT;
using SwapControl.Structure.Configs.Factories;
using SwapControl.Structure.Configs.Interfaces;
using SwapControl.Structure.Enums;
using SwapControl.Structure;
using Quartz.Impl;
using Quartz;
using System.Configuration;
using SwapControl.SQL;
using SwapControl.Structure.Configs.Products;
using SwapControl.SQL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using SwapControl.MT.StructLib;
using SwapControl.JobsScheduler;

internal class Program
{
    static async Task Main(string[] args)
    {
        Logging.Log("Start Program", LogLevel.Info);

        //      --------- START ---------
        //--------- Initializing the config ---------
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "config.xml");
        ConfigSettings configSettings = new ConfigSettings(filePath);

        ConfigFactory MTfactory = new MTSettingsFactory();
        List<Config> mtSettings = new List<Config>();
        configSettings.initConfigSettings("/Config/Common/MT4", MTfactory, mtSettings);

        ConfigFactory SQLfactory = new SQLSettingsFactory();
        List<Config> sqlSettings = new List<Config>();
        configSettings.initConfigSettings("/Config/Common/SQL", SQLfactory, sqlSettings);
        //      --------- END ---------

        MTSettings mTSettings2 = (MTSettings)mtSettings[0];


        //--------- Initializing the Quartz jobs ---------
        var schedulerFactory = new StdSchedulerFactory();
        var scheduler = await schedulerFactory.GetScheduler();

  
        //List<string> executionTimes = mTSettings2.GetArrayOfSwapTimes();
        var executionTimes = new[]
        {
             "0/10 * * ? * *",
        };


        var jobDataMap = new JobDataMap
         {
                { "mtSettings", mtSettings },
                { "connectionSQL", sqlSettings.FirstOrDefault().GetConnectionString() }
         };

        for (int i = 0; i < executionTimes.Length; i++)
        {
            var jobTask = JobBuilder.Create<SyncData>()
                .WithIdentity($"myJob_{i}", "group1")
                .UsingJobData(jobDataMap)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"trigger_{i}", "group1")
                .WithCronSchedule(executionTimes[i])
                .Build();

            await scheduler.ScheduleJob(jobTask, trigger);
        }

        await scheduler.Start();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();

        await scheduler.Shutdown();
        //      --------- END ---------
    }

}