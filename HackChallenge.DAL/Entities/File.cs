using System;

namespace HackChallenge.DAL.Entities
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Size { get; set; }
        public DateTime TimeOfCreating { get; set; }
        public string Text { get; set; }

        public int DirectoryId { get; set; }
        public Directory Directory { get; set; }

        public int CurrentDirectoryId { get; set; }
        public CurrentDirectory CurrentDirectory { get; set; }

        public int PreviousDirectoryId { get; set; }
        public PreviousDirectory PreviousDirectory { get; set; }

        public int MainDirectoryId { get; set; }
        public MainDirectory MainDirectory { get; set; }
    }
}
