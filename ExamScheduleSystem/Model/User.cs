using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.Model
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        public string RoleId { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public Role Role { get; set; }
    }
}
