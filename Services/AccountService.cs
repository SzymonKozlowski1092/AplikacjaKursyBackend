using DziekanatBackend.Database;
using DziekanatBackend.DbModels;
using DziekanatBackend.Exceptions;
using DziekanatBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DziekanatBackend.Services
{
    public interface IAccountService
    {
        void RegisterStudent(RegisterUserDto registerStudentDto);
        void RegisterLecturer(RegisterUserDto registerLecturerDto);
        string GenerateJwt(UserLoginDto loginDto);
    }

    public class AccountService : IAccountService
    {
        private readonly DziekanatDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IConfiguration _configuration;
        public AccountService(DziekanatDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IConfiguration configuration)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _authenticationSettings = authenticationSettings ?? throw new ArgumentNullException(nameof(authenticationSettings));
            _configuration = configuration;
        }

        public void RegisterStudent(RegisterUserDto registerStudentDto)
        {
            if (registerStudentDto == null)
                throw new ArgumentNullException(nameof(registerStudentDto));

            var newStudent = new Student
            {
                FirstName = registerStudentDto.FirstName,
                LastName = registerStudentDto.LastName,
                Email = registerStudentDto.Email,
                DateOfBirth = registerStudentDto.DateOfBirth
            };

            newStudent.PasswordHash = _passwordHasher.HashPassword(newStudent, registerStudentDto.Password);

            _dbContext.Student.Add(newStudent);
            _dbContext.SaveChanges();
        }

        public void RegisterLecturer(RegisterUserDto registerLecturerDto)
        {
            if (registerLecturerDto == null)
                throw new ArgumentNullException(nameof(registerLecturerDto));

            var newLecturer = new Lecturer
            {
                FirstName = registerLecturerDto.FirstName,
                LastName = registerLecturerDto.LastName,
                Email = registerLecturerDto.Email,
                DateOfBirth = registerLecturerDto.DateOfBirth
            };

            newLecturer.PasswordHash = _passwordHasher.HashPassword(newLecturer, registerLecturerDto.Password);

            _dbContext.Lecturer.Add(newLecturer);
            _dbContext.SaveChanges();
        }

        public string GenerateJwt(UserLoginDto loginDto)
        {
            if (loginDto == null)
                throw new ArgumentNullException(nameof(loginDto));

            var user = GetUserByEmail(loginDto.Email);
            if (user == null)
                throw new BadRequestException("Invalid email or password");

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
                throw new BadRequestException("Invalid email or password");

            var role = GetRoleForUser(user);

            return GenerateToken(user, role);
        }

        private User? GetUserByEmail(string email)
        {
            var lecturer = _dbContext.Lecturer.FirstOrDefault(l => l.Email == email);
            if (lecturer != null)
            {
                return lecturer;
            }

            var student = _dbContext.Student.FirstOrDefault(s => s.Email == email);
            if (student != null)
            {
                return student;
            }

            return null;
        }


        private string GetRoleForUser(User user)
        {
            return user switch
            {
                Lecturer => "Lecturer",
                Student => "Student",
                _ => throw new InvalidOperationException("Nieznany typ użytkownika")
            };
        }

        private string GenerateToken(User user, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user is Lecturer lecturer ? lecturer.EmployeeID.ToString() : ((Student)user).Index.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Authentication:JwtIssuer"],
                Audience = _configuration["Authentication:ValidAudience"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(_authenticationSettings.JwtExpireDays),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
