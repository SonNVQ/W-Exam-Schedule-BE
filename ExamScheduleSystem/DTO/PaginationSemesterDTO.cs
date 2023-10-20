using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.DTO
{
    public class PaginatedSemester<Semester>
    {
        public Pagination Pagination { get; set; }
        public List<SemesterDTO> Data { get; set; }

        public static implicit operator PaginatedSemester<Semester>(PaginatedSemester<SemesterDTO> v)
        {
            throw new NotImplementedException();
        }
    }
    public class PaginationSemesterDTO
    {
        public string SemesterId { get; set; }

        public string SemesterName { get; set; }
        public List<MajorDTO> listMajor { get; set; }
        public string Status { get; set; }
    }
}
