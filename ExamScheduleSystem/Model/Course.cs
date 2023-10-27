using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class Course
    {
        [Key]
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string SemesterId { get; set; }
        public string Status { get; set; }
        public Semester Semester { get; set; }
        public ICollection<CourseStudentList> CourseStudentLists { get; set; }
        public ICollection<ExamSchedule> ExamSchedules { get; set; }
    }
}
