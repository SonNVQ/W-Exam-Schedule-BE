using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class StudentListStudent
    {
        [Key]
        public string Username { get; set; }
        [Key]
        public string StudentListId { get; set; }
        public StudentList StudentList { get; set; }
        public Student Student { get; set; }
    }
}
