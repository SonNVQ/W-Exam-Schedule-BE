using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface IClassroomExamScheduleRepository
    {
        bool AddClassroomExamSchedule(ClassroomExamSchedule classroomExamSchedule);
        bool UpdateClassroomExamSchedule(string ClassroomId, List<ClassroomExamSchedule> classroomExamSchedule);
        bool Save();
        bool ClassroomExamScheduleExists(ClassroomExamSchedule c);
        List<ExamSchedule> GetExamScheduleByClassroomId(string classroomId);
    }
}
