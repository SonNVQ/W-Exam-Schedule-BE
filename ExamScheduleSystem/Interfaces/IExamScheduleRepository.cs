using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface IExamScheduleRepository
    {
        ICollection<ExamSchedule> GetExamSchedules();

        ICollection<ExamSchedule> GetExamSchedulesByCourseIDAndExamSlotID(string CourseId, string ExamSlotId);
        ExamSchedule GetExamSchedule(string id);


        bool ExamScheduleExists(string id);

        bool CreateExamSchedule(ExamSchedule examSchedule);

        bool UpdateExamSchedule(ExamSchedule examSchedule);

        bool DeleteExamSchedule(string examSchedule);
        bool Save();
    }
}
