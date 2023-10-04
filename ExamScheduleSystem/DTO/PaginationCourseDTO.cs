namespace ExamScheduleSystem.DTO
{
    public class PaginatedCourse<Course>
    {
        public Pagination Pagination { get; set; }
        public List<PaginationCourseDTO> Data { get; set; }
    }
    public class PaginationCourseDTO
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }

        public string SemesterId { get; set; }
        public string StudentListId { get; set; }
        public string Status { get; set; }
    }
}
