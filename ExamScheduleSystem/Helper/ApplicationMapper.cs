using AutoMapper;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Classroom, PaginationClassroomDTO>().ReverseMap();
            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<ExamSchedule, ExamScheduleDTO>().ReverseMap();
            CreateMap<ExamSlot, ExamSlotDTO>().ReverseMap();
            CreateMap<Major, MajorDTO>().ReverseMap();
            CreateMap<Proctoring, ProctoringDTO>().ReverseMap();
            CreateMap<Semester, SemesterDTO>().ReverseMap();
            CreateMap<StudentList, StudentListDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
