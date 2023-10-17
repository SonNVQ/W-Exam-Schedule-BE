using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class Major
    {
        [Key]
        public string MajorId { get; set; }
        public string MajorName { get; set; }
        public string Status { get; set; }
        public ICollection<SemesterMajor> SemesterMajors { get; set; }
    }
    
}
