
using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.DTO
{
    public class StudentDTO
    {
       [Key]
        public string Username { get; set; }
        public string Email { get; set; }

    }
}
