using System.Collections.Generic;

namespace HackChallenge.DAL.Entities
{
    public class GlobalNetwork
    {
        public int Id { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public Wifi Wifi { get; set; }
    }
}
