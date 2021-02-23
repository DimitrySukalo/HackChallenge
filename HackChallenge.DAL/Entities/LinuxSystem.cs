using System.Collections.Generic;

namespace HackChallenge.DAL.Entities
{
    public class LinuxSystem
    {
        public int Id { get; set; }
        public User User { get; set; }
        public bool IsConnectedTheInternet { get; set; }
        public string IP { get; set; }
        public string MACAddress { get; set; }
        
        public int CurrentDirectoryId { get; set; }
        public CurrentDirectory CurrentDirectory { get; set; }

        public int WifiModuleId { get; set; }
        public WifiModule WifiModule { get; set; }
        public virtual ICollection<Directory> AllDirectories { get; set; }
        public virtual ICollection<Vulnerability> Vulnerabilities { get; set; }
    }
}
