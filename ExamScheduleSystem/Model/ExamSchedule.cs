using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class ExamSchedule
    {
        [Key]
        public string ExamScheduleId { get; set; }
        public string ExamSlotId { get; set; }
        public string ClassroomId { get; set; }
        public string CourseId { get; set; }
        public string ProctoringId { get; set; }
        public string Status { get; set; }
        public ExamSlot ExamSlot { get; set; }    
        public Course Course { get; set; }
        public ICollection<ClassroomExamSchedule> ClassroomExamSchedules { get; set; }
    }
}
