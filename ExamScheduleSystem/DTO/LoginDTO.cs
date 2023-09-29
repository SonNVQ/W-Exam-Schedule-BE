using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.DTO
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
