using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace DziekanatBackend.DbModels
{
    public class Lecturer : User
    {
        [Key]
        public int EmployeeID { get; set; }
        public string? Department { get; set; }
        public string Role { get; set; } = "Lecturer";
        public ICollection<Course>? Courses { get; set; }
    }
}
