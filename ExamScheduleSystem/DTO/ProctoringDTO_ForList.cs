namespace ExamScheduleSystem.DTO
{
    public class ProctoringDTO_ForList
    {
        public string ProctoringId { get; set; }
        public string ProctoringName { get; set; }
        public string Status { get; set; }
        public string Compensation { get; set; }
        public List<ExamSlotDTO> listExamSlot { get; set; }
    }
}
