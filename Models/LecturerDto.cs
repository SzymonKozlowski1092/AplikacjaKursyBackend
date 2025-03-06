using System.ComponentModel.DataAnnotations;

namespace DziekanatBackend.Models
{
    public class LecturerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public int EmployeeID { get; set; }
    }
}
