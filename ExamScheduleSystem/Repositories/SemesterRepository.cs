using ExamScheduleSystem.Data;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Repositories
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly DataContext _context;

        public SemesterRepository(DataContext context)
        {
            _context = context;
        }

        public Semester GetSemester(string id)
        {
            return _context.Semesters.Where(p => p.SemesterId == id).FirstOrDefault();
        }

        public ICollection<Semester> GetSemesters()
        {
            return _context.Semesters.OrderBy(p => p.SemesterId).ToList();
        }

        public bool SemesterExists(string id)
        {
            return _context.Semesters.Any(p => p.SemesterId == id);
        }

        public bool CreateSemester(Semester semester)
        {
            // Change Tracker
            // Add, updating, modifying,
            // connected vs disconnect
            // EntityState.Added 
            _context.Add(semester);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateSemester(Semester semester)
        {
            _context.Update(semester);
            return Save();
        }

        public bool DeleteSemester(Semester semester)
        {
            _context.Remove(semester);
            return Save();
        }
        public List<Major> GetMajorsBySemesterId(string semesterId)
        {
            // Query the database to get students associated with the specified studentListId
            var majors = _context.SemesterMajors
                .Where(sls => sls.SemesterId == semesterId)
                .Select(sls => sls.Major)
                .ToList();
            return majors;
        }
    }
}
