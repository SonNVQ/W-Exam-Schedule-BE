using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.DTO
{
    public class SemesterDTO
    {
        public string SemesterId { get; set; }
        public string SemesterName { get; set; }
        public List<MajorDTO> listMajor { get; set; }
        public string Status { get; set; }
    }
}
