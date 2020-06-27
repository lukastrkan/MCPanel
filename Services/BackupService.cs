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
            var last = _context.Backups.Where(x => x.Type == BackupType.LongTerm).OrderByDescending(x => x.Id).Take(1).ToList();
            var backup = new Backup();
            backup.Filename = filename;
            if (last.Count > 0)
            {
                var time = last[0].DateTime;
                var diff = (DateTime.Now - time).TotalHours;
                if (diff >= 1)
                {
                    backup.Type = BackupType.LongTerm;
                }
            }
            else
            {
                backup.Type = BackupType.LongTerm;
            }

            _context.Backups.Add(backup);
            var longterm = _context.Backups.Where(x => x.Type == BackupType.LongTerm);
            var longtermCount = longterm.Count();
            if (longtermCount >= 7)
            {
                var toDelete = longterm.OrderBy(x => x.Id).Take(longtermCount - 7);
                foreach(var t in toDelete)
                {
                    if (File.Exists(t.Filename))
                    {
                        File.Delete(t.Filename);
                    }
                    _context.Remove(t);
                }
            }

            var normal = _context.Backups.Where(x => x.Type == BackupType.LongTerm);
            var normalCount = normal.Count();
            if (normalCount >= 24)
            {
                var toDelete = normal.OrderBy(x => x.Id).Take(normalCount - 24);
                foreach (var t in toDelete)
                {
                    if (File.Exists(t.Filename))
                    {
                        File.Delete(t.Filename);
                    }
                    _context.Remove(t);
                }
            }
            _context.SaveChanges();
        }

        public void Restore()
        {
            
        }
    }
}
