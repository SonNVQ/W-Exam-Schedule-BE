using ExamScheduleSystem.Model;
using Microsoft.AspNetCore.Identity;

namespace ExamScheduleSystem.Interfaces
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
    }
}
