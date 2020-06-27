using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Hangfire;
using MCPanel.Hubs;

namespace MCPanel.Services
{
    public class MinecraftService : IMinecraftService
    {
        private static Process _process;
        private readonly IBackupService _backupService;
        private readonly ConsoleHub _consoleHub;

        public MinecraftService(ConsoleHub ch, IBackupService bs)
        {
            _consoleHub = ch;
            _backupService = bs;
        }

        public void SetupProcess()
        {
            _process = new Process
            {
                EnableRaisingEvents = true
            };
            _process.Exited += Exited;
            _process.OutputDataReceived += OutputHandler;
            _process.ErrorDataReceived += OutputHandler;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _process.StartInfo.WorkingDirectory = "./minecraft";
                //_process.StartInfo.FileName = "start.bat";
                _process.StartInfo.FileName = "java";
                _process.StartInfo.Arguments = "-Xms1024M -Xmx2048M -jar server.jar nogui";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _process.StartInfo.FileName = "java";
                _process.StartInfo.Arguments = "-Xms1024M -Xmx2048M -jar server.jar nogui";
            }

            _process.StartInfo.RedirectStandardInput = true;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
        }

        public void Execute(string command)
        {
            _process.StandardInput.WriteLine(command);
        }

        public void StartServer()
        {
            SetupProcess();
            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
            _consoleHub.SendConsole("Server started", "green");
            _consoleHub.IsRunning();
        }

        public void StopServer()
        {
            _process.StandardInput.WriteLine("stop");
            var jobId = BackgroundJob.Schedule(() => KillServer(), TimeSpan.FromSeconds(20));
        }

        public void KillServer()
        {
            if (_process == null)
            {
                return;
            }
            if (IsRunning())
            {
                _process.Kill();
                Console.WriteLine("killed");
            }
        }

        public void RestartServer()
        {
            StopServer();
            Thread.Sleep(5000);
            StartServer();
        }

        bool IMinecraftService.IsRunning()
        {
            if (_process == null)
            {
                return false;
            }
            else
            {
                return !_process.HasExited;
            }
        }

        private void Exited(object sender, EventArgs e)
        {
            RecurringJob.RemoveIfExists("backup");
            if (_process.ExitCode != 0)
            {
                _consoleHub.SendConsole("Server crashed", "red");
            }
            else
            {
                _consoleHub.SendConsole("Server stopped", "pink");
                StopProcess();
            }
        }


        private async void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            await _consoleHub.SendConsole(outLine.Data);
            if (outLine.Data == null)
            {

            }
            else if (outLine.Data.Contains("Stopping server"))
            {
                await _consoleHub.SendConsole("Stopping server", "pink");
            }
            else if (outLine.Data.Contains("For help, type \"help\"") && outLine.Data.Contains("Done"))
            {
                RecurringJob.AddOrUpdate("backup", () => Backup(), Cron.Hourly);
            }
        }

        private void StopProcess()
        {
            _process.Dispose();
        }

        public static bool IsRunning()
        {
            try
            {
                return !_process.HasExited;
            }
            catch
            {
                return false;
            }
        }

        public void Backup()
        {
            if (IsRunning())
            {
                Execute("tellraw @a {\"text\": \"Server is going to backuped. You may experience some lag.\", \"color\": \"red\"}");
                Execute("save-all");
                Execute("save-off");
                _backupService.Backup();
                Execute("save-on");
            }
        }

        public static void Stop()
        {
            Stop();
        }
    }
}