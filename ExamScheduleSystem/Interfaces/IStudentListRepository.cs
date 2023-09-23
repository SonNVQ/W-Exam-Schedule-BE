using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface IStudentListRepository
    {
        ICollection<StudentList> GetStudentLists();

        StudentList GetStudentList(string id);


        bool StudentListExists(string id);

        bool CreateStudentList(StudentList studentList);

        bool UpdateStudentList(StudentList studentList);

        bool DeleteStudentList(StudentList studentList);
        bool Save();
    }
}
