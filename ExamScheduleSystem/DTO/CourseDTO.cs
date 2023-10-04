using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.DTO
{
    public class CourseDTO
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }

        public string SemesterId { get; set; }
        public string StudentListId { get; set; }
        public string Status { get; set; }
    }
}
