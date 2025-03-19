using DziekanatBackend.Models;
using DziekanatBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace DziekanatBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IAccountService _accontService;

        public AccountController(IAccountService accontService)
        {
            _accontService = accontService;
        }

        [HttpPost("Register/Student")]
        public ActionResult RegisterStudent([FromBody] RegisterUserDto registerStudentDto)
        {
            _accontService.RegisterStudent(registerStudentDto);
            return Ok();
        }
        [HttpPost("Register/Lecturer")]
        public ActionResult RegisterLecturer([FromBody] RegisterUserDto registerLecturerDto)
        {
            _accontService.RegisterLecturer(registerLecturerDto);
            return Ok();
        }

        [HttpPost("Login")]
        public ActionResult Login([FromBody] UserLoginDto userLoginDto)
        {
            string token = _accontService.GenerateJwt(userLoginDto);
            return Ok(token);
        }
    }
}
