using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface IStudentListRepository
    {
        ICollection<StudentList> GetStudentLists();

        StudentList GetStudentList(string id);
        List<Student> GetStudentsByStudentListId(string studentListId);

        bool StudentListExists(string id);

        bool CreateStudentList(StudentList studentList);

        bool UpdateStudentList(StudentList studentList);

        bool DeleteStudentList(StudentList studentList);
        bool Save();
    }
}
