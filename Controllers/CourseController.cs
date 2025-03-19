using DziekanatBackend.DbModels;
using DziekanatBackend.Models;
using DziekanatBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DziekanatBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly ICoursesService _coursesService;

        public CourseController(ICoursesService coursesService)
        {
            _coursesService = coursesService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Course>> GetCourses()
        {
            var coursesDto = _coursesService.GetCourses();
            return Ok(coursesDto);
        }

        [HttpPost]
        [Authorize(Roles = "Lecturer")]
        public ActionResult AddCourse([FromBody] CreateCourseDto courseDto)
        {
            _coursesService.AddCourse(courseDto);
            return Created();
        }

        [HttpGet("{id}/students")]
        [Authorize(Policy = "LecturerRights")]
        public ActionResult<List<CourseStudentDto>> GetCourseStudents(int id)
        {
            var courseStudentsDto = _coursesService.GetCourseStudents(id);
            return Ok(courseStudentsDto);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<CourseDto> GetCourse(int id)
        {
            var coursesDto = _coursesService.GetCourse(id);
            return Ok(coursesDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Lecturer")]
        public ActionResult UpdateCourse([FromBody] CourseDto courseDto, int id) 
        {
            _coursesService.UpdateCourse(courseDto, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Lecturer")]
        public ActionResult DeleteCourse(int id)
        {
            _coursesService.DeleteCourse(id);
            return NoContent();
        }

        [HttpPost("{courseId}/student/{studentIndex}")]
        [Authorize(Roles = "Student,Lecturer")]
        public ActionResult AddStudentToCourse(int studentIndex, int courseId)
        {
            _coursesService.AddStudentToCourse(studentIndex, courseId);
            return Created();
        }

        [HttpPut("{courseId}/Student/{studentIndex}/ZmienOcene/{grade}")]
        [Authorize(Roles = "Lecturer")]
        public ActionResult ChangeGrade([FromRoute]int studentIndex, [FromRoute] int courseId, [FromRoute] int grade)
        {
            _coursesService.ChangeGrade(studentIndex, courseId, grade);
            return Ok();
        }
    }
}
