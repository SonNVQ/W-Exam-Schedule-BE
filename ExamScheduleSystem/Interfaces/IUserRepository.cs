using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces { 
public interface IUserRepository
    {
        User GetUser();
        void AddUser(User user);
        User GetUserByUsername(string username);
        Student GetStudentByUsername(string username);
        List<User> GetExistingSutdents(List<string> username);
        ICollection<User>  GetUsers();
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        void AddStudent(StudentDTO student);
        void SaveChanges();
        bool Save();
    }
}
