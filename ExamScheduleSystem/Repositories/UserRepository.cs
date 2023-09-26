using ExamScheduleSystem.Data;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using System.Security.Claims;
namespace ExamScheduleSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        

        public User GetUser()
        {
            return _context.User.FirstOrDefault();
        }

        public User GetUserByUsername(string username)
        {
            return _context.User.FirstOrDefault(u => u.Username == username);
        }


        public void AddUser(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
        }
        public ICollection<User> GetUsers()
        {
            return _context.User.ToList();
        }
    }
}
