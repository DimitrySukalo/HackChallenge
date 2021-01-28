using System.Collections.Generic;

namespace HackChallenge.DAL.Entities
{
    public class WifiModule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ModuleMode ModuleMode { get; set; }
        public LinuxSystem LinuxSystem { get; set; }
        public ICollection<Wifi> Wifis { get; set; }
    }
}
