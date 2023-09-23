using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface ISemesterRepository
    {
        ICollection<Semester> GetSemesters();

        Semester GetSemester(string id);


        bool SemesterExists(string id);

        bool CreateSemester(Semester semester);

        bool UpdateSemester(Semester semester);

        bool DeleteSemester(Semester semester);
        bool Save();
    }
}
