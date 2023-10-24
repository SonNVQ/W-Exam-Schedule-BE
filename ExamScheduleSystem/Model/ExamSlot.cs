using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class ExamSlot
    {
        [Key]
        public string ExamSlotId { get; set; }
        public string ExamSlotName { get; set;}
        public string Status { get; set; }
        public string CourseId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public ICollection<ExamSchedule> ExamSchedules { get; set; }
        public ICollection<ExamSlotProctoring> ExamSlotProctorings { get; set; }
    }
}
