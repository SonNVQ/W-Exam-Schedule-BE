using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.DTO
{
    public class RegisterDTO
    {

        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string ConfirmPassword { get; set; } = null!;
    }
}
