using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface IMajorRepository
    {
        ICollection<Major> GetMajors();

        Major GetMajor(string id);


        bool MajorExists(string id);

        bool CreateMajor(Major major);

        bool UpdateMajor(Major major);

        bool DeleteMajor(Major major);
        bool Save();
    }
}
