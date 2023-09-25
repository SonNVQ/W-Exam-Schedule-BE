using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces { 
public interface IUserRepository
    {
        User GetUser();
        void AddUser(User user);
        User GetUserByUsername(string username);

    }
}
