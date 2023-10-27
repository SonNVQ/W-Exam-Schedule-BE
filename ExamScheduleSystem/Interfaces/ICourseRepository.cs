using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface ICourseRepository
    {
        ICollection<Course> GetCourses();
        Course GetCourse(string id);


        bool CourseExists(string id);

        bool CreateCourse(Course course);

        bool UpdateCourse(Course course);

        bool DeleteCourse(Course course);
        bool Save();
        ICollection<StudentList> GetStudentListsByCourseId(string courseId);
    }
}
