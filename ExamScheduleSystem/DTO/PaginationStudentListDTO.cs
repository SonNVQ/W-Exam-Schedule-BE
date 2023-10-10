using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.DTO
{
    public class PaginatedStudentList<StudentList>
    {
        public Pagination Pagination { get; set; }
        public List<PaginationStudentListDTO> Data { get; set; }
    }
    public class PaginationStudentListDTO
    {
        public string StudentListId { get; set; }
        public string CourseId { get; set; }
        public List<StudentDTO> listStudent { get; set; }
        public string Status { get; set; }
    }
}
