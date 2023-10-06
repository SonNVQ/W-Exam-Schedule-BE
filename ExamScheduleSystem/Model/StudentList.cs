using ExamScheduleSystem.DTO;
using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class StudentList
    {
        [Key]
        public string StudentListId { get; set; }
        public List<StudentDTO> ListStudent { get; set; }   
        public string CourseId { get; set; }
        public string Status { get; set; }
        public ICollection<ExamSlot> ExamSlots { get; set; }
        public ICollection<CourseStudentList> CourseStudentLists { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
