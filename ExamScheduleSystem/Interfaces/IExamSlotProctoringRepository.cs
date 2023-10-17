using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface IExamSlotProctoringRepository
    {
        bool AddExamSlotProctoring(ExamSlotProctoring examSlotProctoring);
        bool UpdateExamSlotProctoring(string ExamSlotId, List<ExamSlotProctoring> examSlotProctoring);
        bool Save();
    }
}
