using ExamScheduleSystem.Data;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ExamScheduleSystem.Repositories
{
    public class ProctoringRepository : IProctoringRepository
    {
        private readonly DataContext _context;

        public ProctoringRepository(DataContext context)
        {
            _context = context;
        }

        public Proctoring GetProctoring(string id)
        {
            return _context.Proctorings.Where(p => p.ProctoringId == id).FirstOrDefault();
        }

        public ICollection<Proctoring> GetProctorings()
        {
            return _context.Proctorings.OrderBy(p => p.ProctoringId).ToList();
        }

        public bool ProctoringExists(string id)
        {
            return _context.Proctorings.Any(p => p.ProctoringId == id);
        }

        public bool CreateProctoring(Proctoring proctoring)
        {
            // Change Tracker
            // Add, updating, modifying,
            // connected vs disconnect
            // EntityState.Added 
            _context.Add(proctoring);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateProctoring(Proctoring proctoring)
        {
            _context.Update(proctoring);
            return Save();
        }

        public bool DeleteProctoring(Proctoring proctoring)
        {
            _context.Remove(proctoring);
            return Save();
        }
        public List<ExamSlot> GetExamSlotsByProctoringId(string proctoringId)
        {
            var examSlots = _context.ExamSlotProctorings
                .Where(sls => sls.ProctoringId == proctoringId)
                .Select(sls => sls.ExamSlot)
                .ToList();
            return examSlots;
        }
    }
}
