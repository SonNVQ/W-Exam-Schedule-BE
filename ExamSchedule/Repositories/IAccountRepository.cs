using ExamSchedule.Models;
using Microsoft.AspNetCore.Identity;

namespace ExamSchedule.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
    }
}
