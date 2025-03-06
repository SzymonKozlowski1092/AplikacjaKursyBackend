using AutoMapper;
using DziekanatBackend.Database;
using DziekanatBackend.DbModels;
using DziekanatBackend.Models;
using DziekanatBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DziekanatBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly ILecturerService _lecturerService;
        public LecturerController(ILecturerService lecturerService)
        {
            _lecturerService = lecturerService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Lecturer>> GetLecturers()
        {
            var lecturersDto = _lecturerService.GetLecturers();
            return Ok(lecturersDto);
        }

        [HttpGet("{employeeId}")]
        public ActionResult<Lecturer> GetLecturer([FromRoute] int employeeId)
        {
            var lecturerDto = _lecturerService.GetLecturer(employeeId);
            return Ok(lecturerDto);
        }

        [HttpPut("{employeeID}")]
        public ActionResult UpdateLecturer([FromBody]LecturerDto lecturerDto, [FromRoute]int employeeID)
        {
            UpdateLecturer(lecturerDto, employeeID);

            return Ok();
        }

        /*[HttpPost]
        public ActionResult AddLecturer([FromBody] LecturerDto lecturerDto)
        {
            _lecturerService.AddLecturer(lecturerDto);

            return Created();
        }*/

        [HttpDelete("{employeeID}")]
        public ActionResult DeleteLecturer([FromRoute]int employeeID)
        {
            _lecturerService.DeleteLecturer(employeeID);
            return NoContent();
        }

        [HttpGet("{employeeId}/courses")]
        public ActionResult GetLecturerCourses([FromRoute]int employeeId)
        {
            var coursesDto = _lecturerService.GetLecturerCourses(employeeId);
            return Ok(coursesDto);
        }
    }
}
