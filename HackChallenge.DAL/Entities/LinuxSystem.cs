using System.Collections.Generic;
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
        public bool IsConnectedTheInternet { get; set; }

        [ForeignKey("WifiModule")]
        public int WifiModuleId { get; set; }
        public WifiModule WifiModule { get; set; }

        [ForeignKey("CurrentDirectory")]
        public int CurrentDirId { get; set; }
        public CurrentDirectory CurrentDirectory { get; set; }

        [ForeignKey("PreviousDirectory")]
        public int PreviousDirectoryId { get; set; }
        public PreviousDirectory PreviousDirectory { get; set; }

        [ForeignKey("MainDirectory")]
        public int MainDirectoryId { get; set; }
        public MainDirectory MainDirectory { get; set; }
        public virtual ICollection<Directory> AllDirectories { get; set; }
    }
}
