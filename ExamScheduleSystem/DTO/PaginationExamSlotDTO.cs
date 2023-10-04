using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.DTO
{
    public class PaginatedExamSlot<ExamSlot>
    {
        public Pagination Pagination { get; set; }
        public List<PaginationExamSlotDTO> Data { get; set; }
    }
    public class PaginationExamSlotDTO
    {
        public string ExamSlotId { get; set; }
        public string ExamSlotName { get; set; }
        public string ProctoringId { get; set; }
        public string Status { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public ICollection<ExamSchedule> ExamSchedules { get; set; }
        public Proctoring Proctoring { get; set; }
    }
}
