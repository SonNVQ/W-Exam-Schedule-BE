using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.DTO
{
    public class PaginatedResult<Classroom>
    {
        public Pagination Pagination { get; set; }
        public List<PaginationClassroomDTO> Data { get; set; }
    }
}
