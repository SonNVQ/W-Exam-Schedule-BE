using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class Classroom
    {
        [Key]
        public string ClassroomId { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }

        public ICollection<ClassroomExamSchedule> ClassroomExamSchedules { get; set; }
    }
}
