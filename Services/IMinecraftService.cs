using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCPanel.Services
{
    public interface IMinecraftService
    {
        void SetupProcess();
        void Execute(string command);
        void StartServer();
        void StopServer();
        void RestartServer();
        bool IsRunning();
        public void Backup();
    }
}
