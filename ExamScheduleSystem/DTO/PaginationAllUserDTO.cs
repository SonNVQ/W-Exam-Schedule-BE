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
        public string Status { get; set; }
        public string RoleId { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }

    }
}
