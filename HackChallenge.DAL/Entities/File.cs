using System;

namespace HackChallenge.DAL.Entities
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public double Size { get; set; }
        public DateTime TimeOfCreating { get; set; }
        public string Text { get; set; }

        public int DirectoryId { get; set; }
        public Directory Directory { get; set; }
    }
}
