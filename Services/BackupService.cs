using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace MCPanel.Services
{
    public class BackupService : IBackupService
    {
        public void Backup()
        {
            string SourcePath = "./minecraft/world";
            string DestinationPath = "./temp/world";
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);

            if(!Directory.Exists("./backup"))
            {
                Directory.CreateDirectory("./backup");
            }
            ZipFile.CreateFromDirectory(DestinationPath, $"./backup/backup-{DateTime.Now.ToString("dd-MM-yyyy-HHmm")}.zip", CompressionLevel.Optimal, true);
        }

        public void Restore()
        {
            
        }
    }
}
