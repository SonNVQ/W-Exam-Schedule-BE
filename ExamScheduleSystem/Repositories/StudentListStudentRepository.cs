using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace ExamScheduleSystem.Repositories
{
    public class StudentListStudentRepository : IStudentListStudentRepository
    {
        private readonly DataContext _context;

        public StudentListStudentRepository(DataContext context)
        {
            _context = context;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool UpdateStudentListStudent(string StudentListId, List<StudentListStudent> studentListStudent)
        {
            var existingStudentListStudents = _context.StudentListStudents.Where(s => s.StudentListId == StudentListId).ToList();

            _context.StudentListStudents.RemoveRange(existingStudentListStudents);

            // Next, add the new StudentListStudents
            _context.StudentListStudents.AddRange(studentListStudent);
            return Save();
        }
        public bool AddStudentListStudent(StudentListStudent studentListStudent)
        {
            _context.StudentListStudents.Add(studentListStudent);
            return Save();
        }
    }
}
