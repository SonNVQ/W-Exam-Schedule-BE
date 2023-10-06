using ExamScheduleSystem.Model;
using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.DTO
{
    public class PaginatedAllUser<User>
    {
        public Pagination Pagination { get; set; }
        public List<PaginationAllUserDTO> Data { get; set; }
    }
    public class PaginationAllUserDTO
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