namespace ExamScheduleSystem.Model
{
    public class Semester
    {
        public string SemesterId { get; set; }

        public string SemesterName { get; set; }
        public ICollection<Course> Courses { get; set; }
        public string MajorId { get; set; }
        public string Status { get; set; }


    }
}
