using Microsoft.EntityFrameworkCore;
using System;


namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
        public DateTime DateofBirth { get; set; }

        public int Telephone { get; set; }

    }
}