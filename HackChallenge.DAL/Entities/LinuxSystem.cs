﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackChallenge.DAL.Entities
{
    public class LinuxSystem
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }
        public User User { get; set; }

        public IEnumerable<Directory> Directories { get; set; }
    }
}
