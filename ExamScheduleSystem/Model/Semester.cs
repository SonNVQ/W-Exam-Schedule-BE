using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class Semester
    {
        [Key]
        public string SemesterId { get; set; }
        public string SemesterName { get; set; }
        public string MajorId { get; set; }
        public string Status { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<SemesterMajor> SemesterMajors { get; set; }
    }
}
