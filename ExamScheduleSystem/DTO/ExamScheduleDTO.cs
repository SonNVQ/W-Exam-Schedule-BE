namespace ExamScheduleSystem.DTO
{
    public class ExamScheduleDTO
    {
        public string ExamScheduleId { get; set; }

        public string CourseId { get; set; }
        public string ExamSlotId { get; set; }
        public string ClassroomId { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
