using ExamScheduleSystem.Data;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using Microsoft.EntityFrameworkCore;
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
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }
        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();
        }
        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
        }
    }
}
