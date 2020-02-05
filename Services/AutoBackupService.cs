using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MCPanel.Services
{
    public class AutoBackupService : IHostedService
    {
        Timer timer;
        readonly IMinecraftService minecraftService;
        readonly IBackupService backupService;
        public AutoBackupService(IMinecraftService ms, IBackupService bs)
        {
            minecraftService = ms;
            backupService = bs;
        }
        private void tick(object state)
        {
            if (minecraftService.IsRunning())
            {
                minecraftService.Execute("tellraw @a {\"text\": \"Server is going to backuped. You may experience some lag.\", \"color\": \"red\"}");
                minecraftService.Execute("save-all");
                minecraftService.Execute("save-off");
                backupService.Backup();
                minecraftService.Execute("save-on");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(tick, null, 0, 30*60*1000);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        
    }
}
