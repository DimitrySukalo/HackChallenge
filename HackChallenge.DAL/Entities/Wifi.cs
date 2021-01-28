namespace HackChallenge.DAL.Entities
{
    public class Wifi
    {
        public int Id { get; set; }
        public string BSSID { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public double Speed { get; set; }
        public QualityOfSignal QualityOfSignal { get; set; }
        public Modem Modem { get; set; }
    }
}
