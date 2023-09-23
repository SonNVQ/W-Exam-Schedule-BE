using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace ExamScheduleSystem.Repositories
{
    public class ClassroomRepository : IClassroomRepository
    {
        private readonly DataContext _context;

        public ClassroomRepository(DataContext context) 
        { 
            _context = context;
        }

        public Classroom GetClassroom(string id)
        {
            return _context.Classrooms.Where(p => p.ClassroomId == id).FirstOrDefault();
        }

        public ICollection<Classroom> GetClassrooms()
        {
            return _context.Classrooms.OrderBy(p => p.ClassroomId).ToList();
        }

        public bool ClassroomExists(string id)
        {
            return _context.Classrooms.Any(p => p.ClassroomId == id);
        }

        public bool CreateClassroom(Classroom classroom)
        {
            // Change Tracker
            // Add, updating, modifying,
            // connected vs disconnect
            // EntityState.Added 
            _context.Add(classroom);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateClassroom(Classroom classroom)
        {
            _context.Update(classroom);
            return Save();
        }

        public bool DeleteClassroom(Classroom classroom)
        {
            _context.Remove(classroom);
            return Save();
        }
    }
}
