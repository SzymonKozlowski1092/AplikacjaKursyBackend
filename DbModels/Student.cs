using System.ComponentModel.DataAnnotations;

namespace DziekanatBackend.DbModels
{
    public class Student : User
    {
        [Key]
        public int Index { get; set; }
        public string? Major {  get; set; }
        public string Role { get; set; } = "Student";
        public ICollection<CourseStudent>? Courses { get; set; }
    }
}
