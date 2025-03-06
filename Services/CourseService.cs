using AutoMapper;
using DziekanatBackend.Database;
using DziekanatBackend.DbModels;
using DziekanatBackend.Exceptions;
using DziekanatBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace DziekanatBackend.Services
{
    public interface ICoursesService
    {
        List<CourseDto> GetCourses();
        CourseDto GetCourse(int id);
        void AddCourse(CreateCourseDto course);
        void UpdateCourse(CourseDto course, int courseId);
        void DeleteCourse(int id);
        List<CourseStudentDto> GetCourseStudents(int courseId);
        void AddStudentToCourse(int studentIndex, int courseId);
        void ChangeGrade(int studentIndex, int courseId, int grade);
    }

    public class CourseService : ICoursesService
    {
        private readonly IMapper _mapper;
        private readonly DziekanatDbContext _dbContext;

        public CourseService(DziekanatDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void AddCourse(CreateCourseDto createCourseDto)
        {
            if (createCourseDto is null)
                throw new ArgumentNullException(nameof(createCourseDto));

            var course = _mapper.Map<Course>(createCourseDto);
            _dbContext.Course.Add(course);
            _dbContext.SaveChanges();
        }

        public void AddStudentToCourse(int studentIndex, int courseId)
        {
            var student = GetStudentByIndex(studentIndex);
            var course = GetCourseById(courseId);

            if (_dbContext.CourseStudents.Any(cs => cs.StudentIndex == studentIndex && cs.CourseId == courseId))
                throw new BadRequestException("Student is already enrolled in this course");

            var studentCourse = new CourseStudent
            {
                StudentIndex = studentIndex,
                CourseId = courseId,
                Student = student,
                Course = course
            };

            _dbContext.CourseStudents.Add(studentCourse);
            _dbContext.SaveChanges();
        }

        public void ChangeGrade(int studentIndex, int courseId, int grade)
        {
            var courseStudent = _dbContext.CourseStudents.FirstOrDefault(cs => cs.StudentIndex == studentIndex && cs.CourseId == courseId);

            if (courseStudent is null)
                throw new NotFoundException("Student in course not found");

            courseStudent.Grade = grade;
            _dbContext.SaveChanges();
        }

        public void DeleteCourse(int id)
        {
            var course = GetCourseById(id);

            _dbContext.Course.Remove(course);
            _dbContext.SaveChanges();
        }

        public CourseDto GetCourse(int id)
        {
            var course = _dbContext.Course.Include(c => c.Lecturer).FirstOrDefault(c => c.Id == id);

            if (course is null)
                throw new NotFoundException("Course not found");

            return _mapper.Map<CourseDto>(course);
        }

        public List<CourseDto> GetCourses()
        {
            var courses = _dbContext.Course.Include(c => c.Lecturer).ToList();

            if (!courses.Any())
                throw new NotFoundException("No courses found");

            return _mapper.Map<List<CourseDto>>(courses);
        }

        public List<CourseStudentDto> GetCourseStudents(int courseId)
        {
            var course = _dbContext.Course
                .Include(c => c.CourseStudents)
                .ThenInclude(cs => cs.Student)
                .FirstOrDefault(c => c.Id == courseId);

            if (course is null)
                throw new NotFoundException("Course not found");

            return course.CourseStudents.Select(cs => new CourseStudentDto
            {
                FirstName = cs.Student.FirstName,
                LastName = cs.Student.LastName,
                Index = cs.Student.Index,
                Grade = cs.Grade
            }).ToList();
        }

        public void UpdateCourse(CourseDto courseDto, int courseId)
        {
            var course = GetCourseById(courseId);

            course.Name = courseDto.Name;
            course.Description = courseDto.Description;

            _dbContext.SaveChanges();
        }

        private Student GetStudentByIndex(int studentIndex)
        {
            var student = _dbContext.Student.FirstOrDefault(s => s.Index == studentIndex);
            if (student is null)
                throw new NotFoundException("Student not found");

            return student;
        }

        private Course GetCourseById(int courseId)
        {
            var course = _dbContext.Course.FirstOrDefault(c => c.Id == courseId);
            if (course is null)
                throw new NotFoundException("Course not found");

            return course;
        }
    }
}
