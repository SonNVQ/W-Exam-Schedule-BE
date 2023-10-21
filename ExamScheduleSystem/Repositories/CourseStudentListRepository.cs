using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Repositories
{
    public class CourseStudentListRepository : ICourseStudentListRepository
    {
        private readonly DataContext _context;

        public CourseStudentListRepository(DataContext context)
        {
            _context = context;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool UpdateCourseStudentList(string CourseId, List<CourseStudentList> courseStudentList)
        {
            var existingCourseStudentLists = _context.CourseStudentLists.Where(s => s.CourseId == CourseId).ToList();

            _context.CourseStudentLists.RemoveRange(existingCourseStudentLists);

            // Next, add the new CourseStudentLists
            _context.CourseStudentLists.AddRange(courseStudentList);
            return Save();
        }
        public bool AddCourseStudentList(CourseStudentList courseStudentList)
        {
            _context.CourseStudentLists.Add(courseStudentList);
            return Save();
        }
        public List<StudentList> GetStudentListByCourseId(string courseId)
        {
            // Query the database to get studentLists associated with the specified studentListId
            var studentLists = _context.CourseStudentLists
                .Where(sls => sls.CourseId == courseId)
                .Select(sls => sls.StudentList)
                .ToList();
            return studentLists;
        }
    }
}
