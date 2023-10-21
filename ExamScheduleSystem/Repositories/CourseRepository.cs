using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Repositories
{
        public class CourseRepository : ICourseRepository
    {
            private readonly DataContext _context;

            public CourseRepository(DataContext context)
            {
                _context = context;
            }

            public Course GetCourse(string id)
            {
                return _context.Courses.Where(p => p.CourseId == id).FirstOrDefault();
            }

            public ICollection<Course> GetCourses()
            {
                return _context.Courses.OrderBy(p => p.CourseId).ToList();
            }

            public bool CourseExists(string id)
            {
                return _context.Courses.Any(p => p.CourseId == id);
            }

            public bool CreateCourse(Course course)
            {
                // Change Tracker
                // Add, updating, modifying,
                // connected vs disconnect
                // EntityState.Added 
                _context.Add(course);
                return Save();
            }

            public bool Save()
            {
                var saved = _context.SaveChanges();
                return saved > 0 ? true : false;
            }

            public bool UpdateCourse(Course course)
            {
                _context.Update(course);
                return Save();
            }

            public bool DeleteCourse(Course course)
            {
                _context.Remove(course);
                return Save();
            }
    }
    }
