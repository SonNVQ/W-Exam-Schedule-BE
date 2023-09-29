using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface IExamScheduleRepository
    {
        ICollection<ExamSchedule> GetExamSchedules();

        ExamSchedule GetExamSchedule(string id);


        bool ExamScheduleExists(string id);

        bool CreateExamSchedule(ExamSchedule examSchedule);

        bool UpdateExamSchedule(ExamSchedule examSchedule);

        bool DeleteExamSchedule(ExamSchedule examSchedule);
        bool Save();
    }
}
