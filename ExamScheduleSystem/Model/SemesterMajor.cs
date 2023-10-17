namespace ExamScheduleSystem.Model
{
    public class SemesterMajor
    {
        public string SemesterMajorId { get; set; }
        public string MajorId { get; set; }
        public string SemesterId { get; set; }
        public Major Major { get; set; }
        public Semester Semester { get; set; }
    }
}
