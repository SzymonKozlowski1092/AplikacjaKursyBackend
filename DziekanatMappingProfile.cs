using AutoMapper;
using DziekanatBackend.DbModels;
using DziekanatBackend.Models;

namespace DziakanatBackend
{
    public class DziekanatMappingProfile : Profile
    {
        public DziekanatMappingProfile()
        {
            CreateMap<Lecturer, LecturerDto>();
            CreateMap<LecturerDto, Lecturer>();
            CreateMap<Student, StudentDto>();
            CreateMap<StudentDto, Student>();

            CreateMap<CourseStudent, CourseStudentDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(s => s.Student.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(s => s.Student.LastName));

            CreateMap<CourseStudent, StudentCourseDto>()
                .ForMember(dest => dest.LecturerName, opt => opt.MapFrom(l => $"{l.Course.Lecturer.FirstName} {l.Course.Lecturer.LastName}"))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(l => l.Course.Name));

            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.LecturerName, opt => opt.MapFrom(l => $"{l.Lecturer.FirstName} {l.Lecturer.LastName}"));

            CreateMap<CreateCourseDto, Course>();
        }
    }
}
