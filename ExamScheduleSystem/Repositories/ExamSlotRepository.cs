using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Repositories
{
    public class ExamSlotRepository : IExamSlotRepository
    {
        private readonly DataContext _context;

        public ExamSlotRepository(DataContext context)
        {
            _context = context;
        }

        public ExamSlot GetExamSlot(string id)
        {
            return _context.ExamSlots.Where(p => p.ExamSlotId == id).FirstOrDefault();
        }

        public ICollection<ExamSlot> GetExamSlots()
        {
            return _context.ExamSlots.OrderBy(p => p.ExamSlotId).ToList();
        }

        public bool ExamSlotExists(string id)
        {
            return _context.ExamSlots.Any(p => p.ExamSlotId == id);
        }

        public bool CreateExamSlot(ExamSlot examSlot)
        {
            // Change Tracker
            // Add, updating, modifying,
            // connected vs disconnect
            // EntityState.Added 
            _context.Add(examSlot);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateExamSlot(ExamSlot examSlot)
        {
            _context.Update(examSlot);
            return Save();
        }

        public bool DeleteExamSlot(ExamSlot examSlot)
        {
            _context.Remove(examSlot);
            return Save();
        }
    }
}
