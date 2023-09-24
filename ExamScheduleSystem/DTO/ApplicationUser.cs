using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ExamScheduleSystem.DTO
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
    }
}
