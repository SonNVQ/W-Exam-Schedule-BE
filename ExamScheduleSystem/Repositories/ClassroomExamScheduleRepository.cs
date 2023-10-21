using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Repositories
{
    public class ClassroomExamScheduleRepository : IClassroomExamScheduleRepository
    {
        private readonly DataContext _context;

        public ClassroomExamScheduleRepository(DataContext context)
        {
            _context = context;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool UpdateClassroomExamSchedule(string ClassroomId, List<ClassroomExamSchedule> classroomExamSchedule)
        {
            var existingClassroomExamSchedules = _context.ClassroomExamSchedules.Where(s => s.ClassroomId == ClassroomId).ToList();

            _context.ClassroomExamSchedules.RemoveRange(existingClassroomExamSchedules);

            // Next, add the new ClassroomExamSchedules
            _context.ClassroomExamSchedules.AddRange(classroomExamSchedule);
            return Save();
        }
        public bool AddClassroomExamSchedule(ClassroomExamSchedule classroomExamSchedule)
        {
            _context.ClassroomExamSchedules.Add(classroomExamSchedule);
            return Save();
        }
        public List<ExamSchedule> GetExamScheduleByClassroomId(string classroomId)
        {
            // Query the database to get ExamSchedules associated with the specified classroomId
            var examSchedules = _context.ClassroomExamSchedules
                .Where(sls => sls.ClassroomId == classroomId)
                .Select(sls => sls.ExamSchedule)
                .ToList();
            return examSchedules;
        }
        public bool ClassroomExamScheduleExists(ClassroomExamSchedule c)
        {
            return _context.ClassroomExamSchedules.Any(p => p == c);
        }
    }
}
