using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces { 
public interface IUserRepository
    {
        User GetUser();
        void AddUser(User user);
        User GetUserByUsername(string username);
        ICollection<User>  GetUsers();
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        void SaveChanges();
        bool Save();
    }
}
