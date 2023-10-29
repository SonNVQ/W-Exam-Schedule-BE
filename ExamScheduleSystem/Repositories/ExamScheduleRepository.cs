using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using System.Reflection.Metadata.Ecma335;

namespace ExamScheduleSystem.Repositories
{
    public class ExamScheduleRepository : IExamScheduleRepository
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        

        public ExamScheduleRepository(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
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

        public bool DeleteExamSchedule(string examScheduleId)
        {
            //_context.Remove(_context.ExamSchedules.Where(x => x.ExamScheduleId == examScheduleId));
            var examSchedule = _context.ExamSchedules.FirstOrDefault(e => e.ExamScheduleId == examScheduleId);
            if (examSchedule != null)
            {
                _context.ExamSchedules.Remove(examSchedule);
            }
            return Save();
        }

        public ICollection<ExamSchedule> GetExamSchedulesByCourseIDAndExamSlotID(string CourseId, string ExamSlotId)
        {
            return _context.ExamSchedules.Where(p => (p.CourseId == CourseId && p.ExamSlotId == ExamSlotId)).ToList();
        }


        public ICollection<ExamSchedule> GetExamSchedulesByUsername(string Username)
        {
            var examSchedules = new List<ExamSchedule>();
            // for lecturer
            var lecturer = _context.User
                .Where(x => x.Username == Username && x.RoleId == "LT")
                .FirstOrDefault();
            
            if (lecturer != null)
            {
                var proctoring = _context.Proctorings.Where(x => x.ProctoringName == lecturer.Username).FirstOrDefault();
                var temp = _context.ExamSchedules.Where(P => P.ProctoringId == proctoring.ProctoringId).ToList();
                foreach (var t in temp)
                {
                    examSchedules.Add(t);
                }
            }

            // for student
            var studentLists = _context.StudentListStudents
                .Where(schedule => schedule.Username == Username)
                .ToList();

            
            foreach (var student in studentLists)
            {
                var temp = _context.ExamSchedules.Where(P => P.StudentListId == student.StudentListId).FirstOrDefault();
                examSchedules.Add(temp);
            }

            return examSchedules;
        }
    }
}
