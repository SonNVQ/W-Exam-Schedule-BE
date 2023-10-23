using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.DTO
{
    public class PaginatedExamSchedule<ExamSchedule>
    {
        public Pagination Pagination { get; set; }
        public List<PaginationExamScheduleDTO> Data { get; set; }

        public static implicit operator PaginatedExamSchedule<ExamSchedule>(PaginatedExamSchedule<ExamScheduleDTO> v)
        {
            throw new NotImplementedException();
        }
    }
    public class PaginationExamScheduleDTO
    {
        
        public string ExamScheduleId { get; set; }
        public string ExamSlotId { get; set; }
        public string ClassroomId { get; set; }
        public string CourseId { get; set; }
        public string ProctoringId { get; set; }
        public string StudentListId { get; set; }
        public string Status { get; set; }
    }
}
