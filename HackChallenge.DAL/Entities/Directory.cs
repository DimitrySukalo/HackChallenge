using System;
using System.Collections.Generic;
using System.Linq;

namespace HackChallenge.DAL.Entities
{
    public class Directory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Size { get; }
        public DateTime TimeOfCreating { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Directory> Directories { get; set; }

        public int CurrentDirectoryId { get; set; }
        public CurrentDirectory CurrentDirectory { get; set; }

        public int PreviousDirectoryId { get; set; }
        public PreviousDirectory PreviousDirectory { get; set; }

        public int MainDirectoryId { get; set; }
        public MainDirectory MainDirectory { get; set; }

        public int LinuxSystemId { get; set; }
        public LinuxSystem LinuxSystem { get; set; }

        public double GetSizeOfDir(List<File> files = null)
        {
            if (files == null)
            {
                return Files.Select(f => f.Size).Sum();
            }

            return files.Select(f => f.Size).Sum();
        }
    }
}
