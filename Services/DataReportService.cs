using MCPanel.Hubs;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MCPanel.Services
{
    public class DataReportService : IHostedService
    {
        readonly ControlHub controlHub;
        public DataReportService(ControlHub ch)
        {
            controlHub = ch;
        }
        Timer timer;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(tick, null, 0, 5 * 1000);
            return Task.CompletedTask;
        }

        private void tick(object state)
        {
            //controlHub.IsRunning();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
