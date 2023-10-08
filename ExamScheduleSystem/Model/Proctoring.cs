namespace ExamScheduleSystem.Model
{
    public class Proctoring
    {
        public string ProctoringId { get; set; }
        public string ProctoringName { get; set; }

        public string ProctoringLocation { get; set; }
        public string Compensation { get; set; }
        public string Status { get; set; }

        public ICollection<ExamSlotProctoring> ExamSlotProctorings { get; set; }

    }
}
