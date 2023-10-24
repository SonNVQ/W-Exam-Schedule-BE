using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.DTO
{
    public class PaginatedExamSlot<ExamSlot>
    {
        public Pagination Pagination { get; set; }
        public List<ExamSlotDTO> Data { get; set; }

        public static implicit operator PaginatedExamSlot<ExamSlot>(PaginatedExamSlot<ExamSlotDTO> v)
        {
            throw new NotImplementedException();
        }
    }
    public class PaginationExamSlotDTO
    {
        public string ExamSlotId { get; set; }
        public string ExamSlotName { get; set; }
        public List<ProctoringDTO_NoneList> listProctoring { get; set; }
        public string CourseId { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

    }
}
