using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class Student
    {
        [Key]
        public string Username { get; set; }
        public string Email { get; set; }
        public ICollection<StudentListStudent> StudentListStudents { get; set; }
    }
}
