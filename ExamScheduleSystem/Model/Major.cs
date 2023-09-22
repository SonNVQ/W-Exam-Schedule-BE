namespace ExamScheduleSystem.Model
{
    public class Major
    {
        public string MajorId { get; set; }
        public string MajorName { get; set; }
        public ICollection<Semester> Semesters { get; set; }
    }
    
}
