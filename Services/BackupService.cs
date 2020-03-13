using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using MCPanel.Data;
using MCPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace MCPanel.Services
{
    public class BackupService : IBackupService
    {
        private readonly ApplicationDbContext _context;
        public BackupService(ApplicationDbContext context)
        {
            _context = context;
        }
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
            string filename = $"./backup/backup-{DateTime.Now.ToString("dd-MM-yyyy-HHmm")}.zip";
            ZipFile.CreateFromDirectory(DestinationPath, filename, CompressionLevel.Optimal, true);
            _context.Backups.Add(new Backup
            {
                Filename = filename
            });
            _context.SaveChanges();
        }

        public void Restore()
        {
            
        }
    }
}
