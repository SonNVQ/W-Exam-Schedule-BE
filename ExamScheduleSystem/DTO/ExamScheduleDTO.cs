namespace ExamScheduleSystem.DTO
{
    public class ExamScheduleDTO
    {
        public string ExamScheduleId { get; set; }
        public string ExamSlotId { get; set; }
        public string CourseId { get; set; }
        public string ClassroomId { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
