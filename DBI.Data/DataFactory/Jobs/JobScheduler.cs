using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DBI.Data
{
    public sealed class JobScheduler
    {
        public JobScheduler() { }
 
        public void Scheduler_Start()
        {
            //Execute the mobile notifications job and run it every 15 mins
            TimerCallback callbackDaily = new TimerCallback(Jobs.ProcessMobileNotifications);
            Timer dailyTimer = new Timer(callbackDaily, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
 
        }
    }
}
