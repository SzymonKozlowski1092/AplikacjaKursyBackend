using AutoMapper;
using DziekanatBackend.Database;
using DziekanatBackend.Exceptions;
using DziekanatBackend.Models;
using DziekanatBackend.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DziekanatBackend.Services
{
    public interface ILecturerService
    {
        List<LecturerDto> GetLecturers();
        LecturerDto GetLecturer(int employeeID);
        void AddLecturer(LecturerDto lecturerDto);
        void DeleteLecturer(int employeeID);
        void UpdateLecturer(LecturerDto lecturerDto, int employeeID);
        List<CourseDto> GetLecturerCourses(int employeeID);
    }

    public class LecturerService : ILecturerService
    {
        private readonly IMapper _mapper;
        private readonly DziekanatDbContext _dbContext;

        public LecturerService(IMapper mapper, DziekanatDbContext dbContext)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void AddLecturer(LecturerDto lecturerDto)
        {
            if (lecturerDto is null)
                throw new ArgumentNullException(nameof(lecturerDto));

            var lecturer = _mapper.Map<Lecturer>(lecturerDto);
            _dbContext.Lecturer.Add(lecturer);
            _dbContext.SaveChanges();
        }

        public void DeleteLecturer(int employeeID)
        {
            var lecturer = GetLecturerById(employeeID);

            _dbContext.Lecturer.Remove(lecturer);
            _dbContext.SaveChanges();
        }

        public LecturerDto GetLecturer(int employeeID)
        {
            var lecturer = GetLecturerById(employeeID);
            return _mapper.Map<LecturerDto>(lecturer);
        }

        public List<CourseDto> GetLecturerCourses(int employeeID)
        {
            var lecturer = _dbContext.Lecturer
                .Include(e => e.Courses)
                .FirstOrDefault(l => l.EmployeeID == employeeID);

            if (lecturer is null)
                throw new NotFoundException("Lecturer not found");

            return _mapper.Map<List<CourseDto>>(lecturer.Courses.ToList());
        }

        public List<LecturerDto> GetLecturers()
        {
            var lecturers = _dbContext.Lecturer.ToList();
            if (!lecturers.Any())
                throw new NotFoundException("No lecturers found");

            return _mapper.Map<List<LecturerDto>>(lecturers);
        }

        public void UpdateLecturer(LecturerDto lecturerDto, int employeeID)
        {
            if (lecturerDto is null)
                throw new ArgumentNullException(nameof(lecturerDto));

            var lecturer = GetLecturerById(employeeID);

            lecturer.FirstName = lecturerDto.FirstName;
            lecturer.LastName = lecturerDto.LastName;
            lecturer.Email = lecturerDto.Email;

            _dbContext.SaveChanges();
        }

        private Lecturer GetLecturerById(int employeeID)
        {
            var lecturer = _dbContext.Lecturer.FirstOrDefault(l => l.EmployeeID == employeeID);

            if (lecturer is null)
                throw new NotFoundException("Lecturer not found");

            return lecturer;
        }
    }
}
