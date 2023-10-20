using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class StudentList
    {
        [Key]
        public string StudentListId { get; set; }
        public string CourseId { get; set; }
        public string Status { get; set; }
        public ICollection<CourseStudentList> CourseStudentLists { get; set; }
        public ICollection<StudentListStudent> StudentListStudents { get; set; }
    }
}
