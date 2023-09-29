using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Repositories
{
    public class MajorRepository : IMajorRepository
    {
        private readonly DataContext _context;

        public MajorRepository(DataContext context)
        {
            _context = context;
        }

        public Major GetMajor(string id)
        {
            return _context.Majors.Where(p => p.MajorId == id).FirstOrDefault();
        }

        public ICollection<Major> GetMajors()
        {
            return _context.Majors.OrderBy(p => p.MajorId).ToList();
        }

        public bool MajorExists(string id)
        {
            return _context.Majors.Any(p => p.MajorId == id);
        }

        public bool CreateMajor(Major major)
        {
            // Change Tracker
            // Add, updating, modifying,
            // connected vs disconnect
            // EntityState.Added 
            _context.Add(major);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateMajor(Major major)
        {
            _context.Update(major);
            return Save();
        }

        public bool DeleteMajor(Major major)
        {
            _context.Remove(major);
            return Save();
        }
    }
}
