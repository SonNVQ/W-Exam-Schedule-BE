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
        private string DetermineTimeSlot(TimeSpan startTime)
        {
            // Define your time slot logic here.
            // Example logic: You can use if-else or switch statements to determine the time slot based on the startTime.

            // Replace the example logic with your own business logic to determine the time slot.
            if (startTime >= new TimeSpan(7, 0, 0) && startTime <= new TimeSpan(8, 30, 0))
            {
                return "Slot 1";
            }
            else if (startTime >= new TimeSpan(8, 45, 0) && startTime <= new TimeSpan(10, 15, 0))
            {
                return "Slot 2";
            }
            else if (startTime >= new TimeSpan(10, 30, 0) && startTime <= new TimeSpan(12, 0, 0))
            {
                return "Slot 3";
                // Add more conditions for other time slots as needed.
            }
            else if (startTime >= new TimeSpan(12, 30, 0) && startTime <= new TimeSpan(14, 0, 0))
            {
                return "Slot 4";
            }
            else if (startTime >= new TimeSpan(14, 15, 0) && startTime <= new TimeSpan(15, 45, 0))
            {
                return "Slot 5";
            }
            else if (startTime >= new TimeSpan(16, 00, 0) && startTime <= new TimeSpan(17, 30, 0))
            {
                return "Slot 6";
            }

            // Default case (if no match is found).
            return "Break Time";
        }
    }
}

