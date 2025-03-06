using AutoMapper;
using DziekanatBackend.Database;
using DziekanatBackend.DbModels;
using DziekanatBackend.Models;
using DziekanatBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DziekanatBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public ActionResult<List<Student>>GetStudents() 
        {
            var studentsDto = _studentService.GetStudents();
            return Ok(studentsDto);
        }

        [HttpGet("{index}")]
        public ActionResult<Student> GetStudent([FromRoute]int index) 
        {
            var studentDto = _studentService.GetStudent(index);
            return Ok(studentDto);
        }

        [HttpPut("{index}")]
        public ActionResult UpdateStudent([FromBody] StudentDto studentDto, [FromRoute] int index)
        {
            _studentService.UpdateStudent(studentDto ,index);

            return Ok();
        }

        /*[HttpPost]
        public ActionResult AddStudent(StudentDto studentDto)
        {
            _studentService.AddStudent(studentDto);

            return Created();
        }*/

        [HttpDelete("{index}")]
        public ActionResult DeleteStudent([FromRoute]int index) 
        {
            _studentService.DeleteStudent(index);
            return NoContent();
        }

        [HttpGet("{index}/Courses")]
        public ActionResult<List<StudentCourseDto>> GetStudentCourses(int index)
        {
            var studentCoursesDto = _studentService.GetStudentCourses(index);
            return Ok(studentCoursesDto);
        }
    }
}
