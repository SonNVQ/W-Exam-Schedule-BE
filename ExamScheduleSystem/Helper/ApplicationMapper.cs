using AutoMapper;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<User, PaginationAllUserDTO>().ReverseMap();
            CreateMap<ExamSlot, PaginationExamSlotDTO>().ReverseMap();
            CreateMap<Major, PaginationMajorDTO>().ReverseMap();
            CreateMap<Proctoring, PaginationProctoringDTO>().ReverseMap();
            CreateMap<StudentList, PaginationStudentListDTO>().ReverseMap();
            CreateMap<Semester, PaginationSemesterDTO>().ReverseMap();
            CreateMap<Course, PaginationCourseDTO>().ReverseMap();
            CreateMap<Classroom, PaginationClassroomDTO>().ReverseMap();
            CreateMap<Classroom, ClassroomDTO>().ReverseMap();
            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<ExamSchedule, ExamScheduleDTO>().ReverseMap();
            CreateMap<ExamSlot, ExamSlotDTO>().ReverseMap();
            CreateMap<Major, MajorDTO>().ReverseMap();
            CreateMap<Proctoring, ProctoringDTO>().ReverseMap();
            CreateMap<Semester, SemesterDTO>().ReverseMap();
            CreateMap<StudentList, StudentListDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, PaginationAllUserDTO>().ReverseMap();
            CreateMap<User, EditRoleIdDTO>().ReverseMap();
            CreateMap<User, StudentDTO>().ReverseMap();
        }
    }
}
