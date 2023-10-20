using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Repositories
{
    public class ExamSlotProctoringRepository : IExamSlotProctoringRepository
    {
        private readonly DataContext _context;

        public ExamSlotProctoringRepository(DataContext context)
        {
            _context = context;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool UpdateExamSlotProctoring(string ExamSlotId, List<ExamSlotProctoring> examSlotProctoring)
        {
            var existingExamSlotProctorings = _context.ExamSlotProctorings.Where(s => s.ExamSlotId == ExamSlotId).ToList();

            _context.ExamSlotProctorings.RemoveRange(existingExamSlotProctorings);

            // Next, add the new ExamSlotProctorings
            _context.ExamSlotProctorings.AddRange(examSlotProctoring);
            return Save();
        }
        public bool AddExamSlotProctoring(ExamSlotProctoring examSlotProctoring)
        {
            _context.ExamSlotProctorings.Add(examSlotProctoring);
            return Save();
        }


    }
}
