using System.ComponentModel.DataAnnotations;

namespace DziekanatBackend.DbModels
{
    public class CourseStudent
    {
        public int CourseId { get; set; }

        public Course? Course { get; set; }

        public int StudentIndex { get; set; }
        public Student? Student { get; set; }
        
        [Range(0,5, ErrorMessage = "Grade must be greater than 1 and less than 6")]
        public double? Grade { get; set; }

    }
}
