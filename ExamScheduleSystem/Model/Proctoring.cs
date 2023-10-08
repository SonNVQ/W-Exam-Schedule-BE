using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class Proctoring
    {
        [Key]
        public string ProctoringId { get; set; }
        public string ProctoringName { get; set; }
        public string ExamSlotId { get; set; }
        public string Compensation { get; set; }
        public string Status { get; set; }

        public ICollection<ExamSlotProctoring> ExamSlotProctorings { get; set; }

    }
}
