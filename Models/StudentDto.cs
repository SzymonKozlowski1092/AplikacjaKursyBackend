using System.ComponentModel.DataAnnotations;

namespace DziekanatBackend.Models
{
    public class StudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public int Index { get; set; }
    }
}
