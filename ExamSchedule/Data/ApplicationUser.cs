using Microsoft.AspNetCore.Identity;

namespace ExamSchedule.Data
{
    public class ApplicationUser :IdentityUser
    {
        public string FisrtName { get; set; } = null!;
        public string LastName { get; set; } = null!; 

    }
}
