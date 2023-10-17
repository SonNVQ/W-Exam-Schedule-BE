using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface IExamSlotRepository
    {
        ICollection<ExamSlot> GetExamSlots();

        ExamSlot GetExamSlot(string id);

        List<Proctoring> GetProctoringsByExamSlotId(string examSlotId);
        bool ExamSlotExists(string id);

        bool CreateExamSlot(ExamSlot examSlot);

        bool UpdateExamSlot(ExamSlot examSlot);

        bool DeleteExamSlot(ExamSlot examSlot);
        bool Save();
    }
}
