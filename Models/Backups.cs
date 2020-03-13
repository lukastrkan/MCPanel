using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCPanel.Models
{
    public enum BackupType
    {
        Normal, LongTerm, Manual
    }
    public class Backup
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public DateTime DateTime { get; set; }
        public BackupType Type { get; set; }

        public Backup()
        {
            DateTime = DateTime.Now;
            if ((DateTime.Hour == 0 && DateTime.Minute == 0) || (DateTime.Hour == 12 && DateTime.Minute == 0))
            {
                Type = BackupType.LongTerm;
            }
            else
            {
                Type = BackupType.Normal;
            }
        }

    }
}
