﻿using System;
using System.ServiceProcess;
using System.Threading;
using Operation;
using Conf;

namespace TongXineSyncService
{
    public partial class TXService : ServiceBase
    {
        private Timer timer;

        public TXService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Configuration.Load();
            DateTime now = DateTime.Now;
            TimeSpan timeSpan = Configuration.StartDate - now;

            long totalMinutes = timeSpan.Days * 24 * 60 + timeSpan.Hours * 60 + timeSpan.Minutes;

            //执行周期分钟
            long dueTime = 0L;
            long interval = Configuration.Interval == 0 ? 30L : Configuration.Interval;

            if (totalMinutes <= 0)
            {
                long point1 = Math.Abs(totalMinutes) / interval + 1;
                dueTime = point1 * interval - Math.Abs(totalMinutes);
            }
            else
            {
                dueTime = totalMinutes;
            }

            Loger.log.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t服务已开启");
            Loger.log.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t配置文件加载成功");

            timer = new Timer(new TimerCallback(DataCollection.GetMinuteResultCollection), null, dueTime * 60 * 1000, interval * 1000 * 60);
        }

        protected override void OnStop()
        {
            //Loger.log.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t服务已关闭");
        }
    }
}