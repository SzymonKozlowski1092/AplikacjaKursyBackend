using System.ComponentModel.DataAnnotations;

namespace DziekanatBackend.DbModels
{
    public abstract class User
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
    }
}
