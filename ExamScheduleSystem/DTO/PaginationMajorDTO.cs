using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.DTO
{
    public class PaginatedMajor<Major>
    {
        public Pagination Pagination { get; set; }
        public List<PaginationMajorDTO> Data { get; set; }

    }
    public class PaginationMajorDTO
    {
        public string MajorId { get; set; }
        public string MajorName { get; set; }
        public string Status { get; set; }
        public ICollection<Semester> Semesters { get; set; }
    }
}
