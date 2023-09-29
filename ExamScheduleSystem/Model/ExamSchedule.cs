using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class ExamSchedule
    {
        [Key]
        public string ExamScheduleId { get; set; }

        public string CourseId { get; set; }
        public string ExamSlotId { get; set; }
        public string ClassroomId { get; set; }
        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public Course Course { get; set; }
        public ExamSlot ExamSlot { get; set; }    
        public ICollection<ClassroomExamSchedule> ClassroomExamSchedules { get; set; }
    }
}
