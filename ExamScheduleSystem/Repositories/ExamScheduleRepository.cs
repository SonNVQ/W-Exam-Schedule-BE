using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Repositories
{
    public class ExamScheduleRepository : IExamScheduleRepository
    {
        private readonly DataContext _context;

        public ExamScheduleRepository(DataContext context)
        {
            _context = context;
        }

        public ExamSchedule GetExamSchedule(string id)
        {
            return _context.ExamSchedules.Where(p => p.ExamScheduleId == id).FirstOrDefault();
        }

        public ICollection<ExamSchedule> GetExamSchedules()
        {
            return _context.ExamSchedules.OrderBy(p => p.ExamScheduleId).ToList();
        }

        public bool ExamScheduleExists(string id)
        {
            return _context.ExamSchedules.Any(p => p.ExamScheduleId == id);
        }

        public bool CreateExamSchedule(ExamSchedule examSchedule)
        {
            // Change Tracker
            // Add, updating, modifying,
            // connected vs disconnect
            // EntityState.Added 
            _context.Add(examSchedule);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateExamSchedule(ExamSchedule examSchedule)
        {
            _context.Update(examSchedule);
            return Save();
        }

        public bool DeleteExamSchedule(ExamSchedule examSchedule)
        {
            _context.Remove(examSchedule);
            return Save();
        }
    }
}
