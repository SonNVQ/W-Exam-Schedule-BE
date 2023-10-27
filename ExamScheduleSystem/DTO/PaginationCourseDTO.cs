using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.DTO
{
    public class PaginatedCourse<Course>
    {
        public Pagination Pagination { get; set; }
        public List<CourseDTO> Data { get; set; }

        public static implicit operator PaginatedCourse<Course>(PaginatedCourse<CourseDTO> v)
        {
            throw new NotImplementedException();
        }
    }
    public class PaginationCourseDTO
        {
            public string CourseId { get; set; }
            public string CourseName { get; set; }

            public string SemesterId { get; set; }
            public List<StudentListDTO> listStudentList { get; set; }
            public string Status { get; set; }
        }
}
