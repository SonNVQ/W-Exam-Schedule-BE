using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.DTO
{
    public class ExamSlotDTO
    {
        public string ExamSlotId { get; set; }
        public string ExamSlotName { get; set; }
        public List<ProctoringDTO> listProctoring { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
