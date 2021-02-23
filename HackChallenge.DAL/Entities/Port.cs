using System.Collections.Generic;

namespace HackChallenge.DAL.Entities
{
    public class Port
    {
        public int Id { get; set; }
        public PortState PortState { get; set; }
        public TypeOfPort TypeOfPort { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public virtual ICollection<Vulnerability> Vulnerabilities { get; set; }
    }
}
