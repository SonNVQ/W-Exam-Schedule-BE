using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface ICourseStudentListRepository
    {
        bool AddCourseStudentList(CourseStudentList courseStudentList);
        bool UpdateCourseStudentList(string CourseId, List<CourseStudentList> courseStudentList);
        bool Save();
        List<StudentList> GetStudentListByCourseId(string courseId);
    }
}
