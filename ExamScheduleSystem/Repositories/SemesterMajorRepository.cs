using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Repositories
{
    public class SemesterMajorRepository : ISemesterMajorRepository
    {
        private readonly DataContext _context;

        public SemesterMajorRepository(DataContext context)
        {
            _context = context;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool UpdateSemesterMajor(string SemesterId, List<SemesterMajor> semesterMajor)
        {
            var existingSemesterMajors = _context.SemesterMajors.Where(s => s.SemesterId == SemesterId).ToList();

            _context.SemesterMajors.RemoveRange(existingSemesterMajors);

            // Next, add the new StudentListStudents
            _context.SemesterMajors.AddRange(semesterMajor);
            return Save();
        }
        public bool AddSemesterMajor(SemesterMajor semesterMajor)
        {
            _context.SemesterMajors.Add(semesterMajor);
            return Save();
        }
    }
}
