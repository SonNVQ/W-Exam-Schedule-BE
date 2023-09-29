using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Repositories
{
    public class StudentListRepository : IStudentListRepository
    {
        private readonly DataContext _context;

        public StudentListRepository(DataContext context)
        {
            _context = context;
        }

        public StudentList GetStudentList(string id)
        {
            return _context.StudentLists.Where(p => p.StudentListId == id).FirstOrDefault();
        }

        public ICollection<StudentList> GetStudentLists()
        {
            return _context.StudentLists.OrderBy(p => p.StudentListId).ToList();
        }

        public bool StudentListExists(string id)
        {
            return _context.StudentLists.Any(p => p.StudentListId == id);
        }

        public bool CreateStudentList(StudentList studentList)
        {
            // Change Tracker
            // Add, updating, modifying,
            // connected vs disconnect
            // EntityState.Added 
            _context.Add(studentList);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateStudentList(StudentList studentList)
        {
            _context.Update(studentList);
            return Save();
        }

        public bool DeleteStudentList(StudentList studentList)
        {
            _context.Remove(studentList);
            return Save();
        }
    }
}
