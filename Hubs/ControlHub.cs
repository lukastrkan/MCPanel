using MCPanel.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCPanel.Hubs
{
    public class ControlHub : Hub
    {
        readonly IMinecraftService minecraftService;
        public ControlHub(IMinecraftService ms)
        {
            minecraftService = ms;
        }
        public Task ExecuteCommand(string text)
        {
            minecraftService.Execute(text);
            return Task.CompletedTask;
        }

        public Task Start()
        {
            minecraftService.StartServer();
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            minecraftService.StopServer();
            return Task.CompletedTask;
        }

        public Task Restart()
        {
            minecraftService.RestartServer();
            return Task.CompletedTask;
        }
                
    }
}
