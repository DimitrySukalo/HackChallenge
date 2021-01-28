using System.Collections.Generic;

namespace HackChallenge.DAL.Entities
{
    public class Modem
    {
        public int Id { get; set; }
        public string IPAddress { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Wifi> Wifis { get; set; }
        public virtual ICollection<LinuxSystem> LinuxSystems { get; set; }
    }
}
