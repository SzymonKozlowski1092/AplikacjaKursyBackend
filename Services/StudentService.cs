using AutoMapper;
using DziekanatBackend.Database;
using DziekanatBackend.Exceptions;
using DziekanatBackend.Models;
using DziekanatBackend.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DziekanatBackend.Services
{
    public interface IStudentService
    {
        List<StudentDto> GetStudents();
        StudentDto GetStudent(int index);
        void AddStudent(StudentDto studentDto);
        void DeleteStudent(int index);
        void UpdateStudent(StudentDto studentDto, int index);
        List<StudentCourseDto> GetStudentCourses(int index);
    }

    public class StudentService : IStudentService
    {
        private readonly DziekanatDbContext _dbContext;
        private readonly IMapper _mapper;

        public StudentService(DziekanatDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public List<StudentDto> GetStudents()
        {
            var students = _dbContext.Student.ToList();
            if (!students.Any())
            {
                throw new NotFoundException("No students found");
            }

            return _mapper.Map<List<StudentDto>>(students);
        }

        public StudentDto GetStudent(int index)
        {
            var student = GetStudentByIndex(index);
            return _mapper.Map<StudentDto>(student);
        }

        public void AddStudent(StudentDto studentDto)
        {
            if (studentDto == null)
                throw new ArgumentNullException(nameof(studentDto));

            var student = _mapper.Map<Student>(studentDto);
            _dbContext.Student.Add(student);
            _dbContext.SaveChanges();
        }

        public void DeleteStudent(int index)
        {
            var student = GetStudentByIndex(index);

            _dbContext.Student.Remove(student);
            _dbContext.SaveChanges();
        }

        public void UpdateStudent(StudentDto studentDto, int index)
        {
            if (studentDto == null)
                throw new ArgumentNullException(nameof(studentDto));

            var student = GetStudentByIndex(index);

            student.FirstName = studentDto.FirstName;
            student.LastName = studentDto.LastName;
            student.Email = studentDto.Email;

            _dbContext.SaveChanges();
        }

        public List<StudentCourseDto> GetStudentCourses(int index)
        {
            var student = _dbContext.Student
                .Include(s => s.Courses)
                .ThenInclude(cs => cs.Course)
                .ThenInclude(c => c.Lecturer)
                .FirstOrDefault(s => s.Index == index);

            if (student == null)
                throw new NotFoundException("Student not found");

            return student.Courses.Select(cs => new StudentCourseDto
            {
                Id = cs.CourseId,
                Name = cs.Course.Name,
                Grade = cs.Grade ?? 0,
                LecturerName = $"{cs.Course.Lecturer.FirstName} {cs.Course.Lecturer.LastName}"
            }).ToList();
        }

        private Student GetStudentByIndex(int index)
        {
            var student = _dbContext.Student.FirstOrDefault(s => s.Index == index);

            if (student == null)
                throw new NotFoundException("Student not found");

            return student;
        }
    }
}
