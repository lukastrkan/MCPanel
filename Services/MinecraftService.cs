using MCPanel.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using Hangfire;

namespace MCPanel.Services
{
    public class MinecraftService : IMinecraftService
    {
        public MinecraftService(ConsoleHub ch)
        {            
            consoleHub = ch;                              
        }
                
        readonly ConsoleHub consoleHub;        
        static Process process;
        static bool setup = false;
        public static bool Running = false;
        public void SetupProcess()
        {
            process = new Process();
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(Exited);            
            process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                process.StartInfo.WorkingDirectory = "./minecraft";
                process.StartInfo.FileName = "start.bat";
                Console.WriteLine(Directory.GetCurrentDirectory());
                    
            }
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                process.StartInfo.FileName = "/bin/bash";
                process.StartInfo.Arguments = "-c \"cd minecraft && ./start.sh\"";
            }            
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;            
        }

        private void Exited(object sender, EventArgs e)
        {            
            consoleHub.IsRunning();            
            if(process.ExitCode != 0)
            {
                consoleHub.SendConsole("Server crashed", "red");
                Thread.Sleep(2000);                
            }
            else
            {
                consoleHub.SendConsole("Server stopped", "pink");
                Thread.Sleep(2000);
                StopProcess();
            }
            
        }
                

        private async void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {            
            await consoleHub.SendConsole(outLine.Data);
            if (outLine.Data == null)
            {
                return;
            }
            else if (outLine.Data.Contains("Stopping server"))
            {
                Running = false;                
                await consoleHub.SendConsole("Stopping server", "pink");                
                //StopProcess();
            }
            else if (outLine.Data.Contains("For help, type \"help\"") && outLine.Data.Contains("Done"))
            {
                Running = true;
            }
        }

        void StopProcess()
        {           
            //process.CancelErrorRead();
            //process.CancelOutputRead();            
            //process.WaitForExit();
            process.Dispose();
            //consoleHub.IsRunning();
        }

        public void Execute(string command)
        {            
            process.StandardInput.WriteLine(command);                     
        }

        public void StartServer()
        {
            SetupProcess();
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            consoleHub.SendConsole("Server started", "green");
            consoleHub.IsRunning();
           // RecurringJob.AddOrUpdate(recurringJobId: "backup", () => Console.WriteLine("Recurring!"), Cron.Minutely);
        }

        public void StopServer()
        {
            //RecurringJob.RemoveIfExists("backup");
            process.StandardInput.WriteLine("stop");
            Thread.Sleep(10000);            
        }

        public void RestartServer()
        {
            StopServer();
            Thread.Sleep(5000);
            StartServer();
        }

        public static bool IsRunning()
        {
            try
            {
                return !process.HasExited;
            }
            catch
            {
                return false;
            }
            
        }

        bool IMinecraftService.IsRunning()
        {
            try
            {
                return !process.HasExited;
            }
            catch
            {
                return false;
            }
        }
    }

}
