using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.DTO
{
    public class PaginatedSemester<Semester>
    {
        public Pagination Pagination { get; set; }
        public List<PaginationSemesterDTO> Data { get; set; }
    }
    public class PaginationSemesterDTO
    {
        public string SemesterId { get; set; }

        public string SemesterName { get; set; }
        public ICollection<Course> Courses { get; set; }
        public string MajorId { get; set; }
        public string Status { get; set; }
    }
}
