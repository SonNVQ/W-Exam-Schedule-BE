using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class ExamSlotProctoring
    {
        [Key]
        public string ExamSlotId { get; set; }
        [Key]
        public string ProctoringId { get; set; }
        public ExamSlot ExamSlot { get; set; }
        public Proctoring Proctoring { get; set; }
    }
}
