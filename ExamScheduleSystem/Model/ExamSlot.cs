namespace ExamScheduleSystem.Model
{
    public class ExamSlot
    {
        public string ExamSlotId { get; set; }
        public string ExamSlotName { get; set; }
        public string ProctoringId { get; set; }
        public string ClassroomId { get; set; }
        public string CourseId { get; set; }
        public string StudentListId { get; set; }   
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public StudentList StudentList { get; set; }
        public Proctoring Proctoring { get; set; }
        public Course Course { get; set; }
        public Classroom Classroom { get; set; }
        public ICollection<ExamSlotExamSchedule> ExamSlotExamSchedules { get; set; }
    }
}
