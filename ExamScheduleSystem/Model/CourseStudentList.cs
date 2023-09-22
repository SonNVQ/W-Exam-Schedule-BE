namespace ExamScheduleSystem.Model
{
    public class CourseStudentList
    {
        public string CourseId { get; set; }

        public string StudentListId { get; set;}

        public string CourseStudentListId { get; set; }

        public Course Course { get; set;}

        public StudentList StudentList { get; set;}
    }
}
