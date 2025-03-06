using DziekanatBackend.DbModels;
using System.ComponentModel.DataAnnotations;

namespace DziekanatBackend.Models
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LecturerName {  get; set; }
    }
}
