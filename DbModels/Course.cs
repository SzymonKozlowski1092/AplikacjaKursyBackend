using System.ComponentModel.DataAnnotations;

namespace DziekanatBackend.DbModels
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public int LecturerId { get; set; }
        public Lecturer? Lecturer { get; set; }
        public ICollection<CourseStudent>? CourseStudents { get; set; }
    }
}
