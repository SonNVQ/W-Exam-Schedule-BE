using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class ExamSchedule
    {
        [Key]
        public string ExamScheduleId { get; set; }
        public List<ExamSlot> ExamSlots { get; set; }
        public DateTime Date { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public ICollection<ExamSlotExamSchedule> ExamSlotExamSchedules { get; set; }
    }
}
