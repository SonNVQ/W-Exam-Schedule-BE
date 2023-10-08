namespace ExamScheduleSystem.Model
{
    public class ExamSlotProctoring
    {
        public string ExamSlotProctoringId { get; set; }
        public string ExamSlotId { get; set; }
        public string ProctoringId { get; set; }
        public ExamSlot ExamSlot { get; set; }
        public Proctoring Proctoring { get; set; }
    }
}
