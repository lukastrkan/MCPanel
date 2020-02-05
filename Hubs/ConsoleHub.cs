using MCPanel.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCPanel.Hubs
{
    public class ConsoleHub : Hub
    {        
        public Task SendConsole(string r, string col = null)
        {
            if(r != null)
            {
                string color;
                if (r.Contains("WARN"))
                {
                    color = "orange";
                }
                else if (r.Contains("ERROR"))
                {
                    color = "red";
                }
                else if(col != null)
                {
                    color = col;
                }
                else
                {
                    color = "white";
                }
                if ((!r.Contains("-jar") && !r.Contains("java")))
                {
                    if (r != "")
                    {
                        return Clients.All.SendAsync("ReceiveConsole", $"<span style=\"color: {color};\">{r}</span>");
                    }
                }
            }            
            return Task.CompletedTask;
        }

        public Task IsRunning()
        {
            try
            {
                return Clients.All.SendAsync("IsRunning", MinecraftService.IsRunning());
            }
            catch
            {

            }
            return Task.CompletedTask;
        }
    }
}
