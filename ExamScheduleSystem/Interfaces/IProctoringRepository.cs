using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface IProctoringRepository
    {
        ICollection<Proctoring> GetProctorings();

        Proctoring GetProctoring(string id);

        List<ExamSlot> GetExamSlotsByProctoringId(string proctoringId);
        bool ProctoringExists(string id);

        bool CreateProctoring(Proctoring proctoring);

        bool UpdateProctoring(Proctoring proctoring);

        bool DeleteProctoring(Proctoring proctoring);
        bool Save();
    }
}
