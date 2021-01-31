﻿using System;
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
        public CurrentDirectory CurrentDirectory { get; set; }

        public double GetSizeOfDir()
        {
            return Files.Select(f => f.Size).Sum();
        }
    }
}
