using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExamScheduleSystem.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }
            public async Task<string> SignInAsync(SignInModel model)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email.ToString(), model.Password.ToString(), false, false);

                if (!result.Succeeded)
                {
                    return string.Empty;
                }

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
                    new Claim(ClaimTypes.Name, model.Email)
                };

                var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };
            return await userManager.CreateAsync(user, model.Password);
        }
    }
   }

