namespace ExamScheduleSystem.Model
{
    public class StudentList
    {
        public string StudentListId { get; set; }
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public ICollection<CourseStudentList> CourseStudentLists { get; set; }
    }
}
