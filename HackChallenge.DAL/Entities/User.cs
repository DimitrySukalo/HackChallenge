﻿using System.ComponentModel.DataAnnotations;

namespace HackChallenge.DAL.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public long ChatId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CountOfIncorrectLoginData { get; set; }
        public int CountOfCorrectUserName { get; set; }
        public bool isAuthorized { get; set; }
        public LinuxSystem LinuxSystem { get; set; }
        public bool HaveLinuxPermission { get; set; }

        public override string ToString()
        {
            return UserName;
        }
    }
}
