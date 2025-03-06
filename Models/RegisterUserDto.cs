﻿using System.ComponentModel.DataAnnotations;

namespace DziekanatBackend.Models
{
    public class RegisterUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
