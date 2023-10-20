using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface ISemesterMajorRepository
    {
        bool AddSemesterMajor(SemesterMajor semesterMajor);
        bool UpdateSemesterMajor(string SemesterId, List<SemesterMajor> semesterMajor);
        bool Save();
    }
}
