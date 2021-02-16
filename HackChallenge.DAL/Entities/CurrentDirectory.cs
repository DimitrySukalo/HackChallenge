namespace HackChallenge.DAL.Entities
{
    public class CurrentDirectory
    {
        public int Id { get; set; }
        public int DirectoryId { get; set; }
        public virtual Directory Directory { get; set; }

        public LinuxSystem LinuxSystem { get; set; }
    }
}
