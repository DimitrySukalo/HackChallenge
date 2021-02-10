using System.Collections.Generic;

namespace HackChallenge.DAL.Entities
{
    public class LinuxSystem
    {
        public int Id { get; set; }
        public User User { get; set; }
        public bool IsConnectedTheInternet { get; set; }

        public int WifiModuleId { get; set; }
        public WifiModule WifiModule { get; set; }

        public int CurrentDirId { get; set; }
        public CurrentDirectory CurrentDirectory { get; set; }

        public int PreviousDirectoryId { get; set; }
        public PreviousDirectory PreviousDirectory { get; set; }

        public int MainDirectoryId { get; set; }
        public MainDirectory MainDirectory { get; set; }
        public virtual ICollection<Directory> AllDirectories { get; set; }
    }
}
