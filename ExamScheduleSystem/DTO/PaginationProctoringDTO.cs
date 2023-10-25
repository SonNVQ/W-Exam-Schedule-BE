using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.DTO
{
    public class PaginatedProctoring<Proctoring>
    {
        public Pagination Pagination { get; set; }
        public List<ProctoringDTO> Data { get; set; }
    }
    public class PaginationProctoringDTO
    {
        public string ProctoringId { get; set; }
        public string ProctoringName { get; set; }
        public string Compensation { get; set; }
        public string Status { get; set; }
        public List<ExamSlotDTO> listExamSlot { get; set; }
    }
}
