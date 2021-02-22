using System.Collections.Generic;

namespace HackChallenge.DAL.Entities
{
    public class Wifi
    {
        public int Id { get; set; }
        public string BSSID { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public double Speed { get; set; }
        public int Channel { get; set; }
        public EncryptionType EncryptionType { get; set; }
        public QualityOfSignal QualityOfSignal { get; set; }
        public Cipher Cipher { get; set; }
        
        public virtual  ICollection<WifiModule> WifiModules { get; set; }

        public int GlobalNetworkId { get; set; }
        public GlobalNetwork GlobalNetwork { get; set; }
    }
}
