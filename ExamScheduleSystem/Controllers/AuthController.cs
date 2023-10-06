using AutoMapper;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using ExamScheduleSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ExamScheduleSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthController(IConfiguration configuration, IUserRepository userService, IMapper mapper)
        {
            _configuration = configuration;
            _userRepository = userService;
            _mapper = mapper;
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _userRepository.GetUser();
            return Ok(userName);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            var existingUser = _userRepository.GetUserByUsername(request.Username);

            if (existingUser != null)
            {
                return BadRequest("User already exists.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Status = request.Status,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RefreshToken = "your-refresh-token",
                TokenCreated = DateTime.Now,
                TokenExpires = DateTime.Now.AddDays(7),
                RoleId = request.RoleId // Đặt giá trị RoleId ở đây
            };

            // Lưu thông tin người dùng vào cơ sở dữ liệu thông qua UserRepository
            _userRepository.AddUser(user);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDTO request)
        {
            var user = _userRepository.GetUserByUsername(request.Username);

            if (user == null)
            {
                return BadRequest("Invalid username or password.");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(new { Token = token, Username = user.Username, Role = user.RoleId });
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }
        [HttpGet("AllStudents")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetAllStudents()
        {
            var users = _userRepository.GetUsers().Where(u => u.RoleId == "ST").ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(users);
        }
        [HttpGet("AllLecturers")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetAllLecturers()
        {
            var users = _userRepository.GetUsers().Where(u => u.RoleId == "LT").ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(users);
        }



        [HttpGet("AllUser")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = "", [FromQuery] string? sortBy = "", [FromQuery] bool isAscending = true)
        {
            /*var users = _userRepository.GetUsers().ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(users);*/
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }

            var allUsers = _userRepository.GetUsers().ToList();
            IEnumerable<User> filteredUsers = allUsers;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filteredUsers = allUsers.Where(user =>
                    user.Username.ToUpper().Contains(keyword.ToUpper()) ||
                    user.RoleId.ToUpper().Contains(keyword.ToUpper())
                );
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "username":
                        filteredUsers = isAscending
                            ? filteredUsers.OrderBy(user => user.Username)
                            : filteredUsers.OrderByDescending(user => user.Username);
                        break;
                    case "roleId":
                        filteredUsers = isAscending
                            ? filteredUsers.OrderBy(user => user.RoleId)
                            : filteredUsers.OrderByDescending(user => user.RoleId);
                        break;
                    default:
                        return BadRequest("Invalid sortBy parameter.");
                }
            }
            int totalCount = filteredUsers.Count();
            var pagedUsers = filteredUsers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => _mapper.Map<PaginationAllUserDTO>(c))
                .ToList();

            var pagination = new Pagination
            {
                currentPage = page,
                pageSize = pageSize,
                totalPage = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize))
            };


            PaginatedAllUser<User> paginatedResult = new PaginatedAllUser<User>
            {
                Data = pagedUsers,
                Pagination = pagination
            };

            return Ok(paginatedResult);
        }


        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.RoleId)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        [HttpPut("UpdatePassword")]
        public async Task<ActionResult<User>> UpdatePassword(UserUpdateDTO request)
        {
            var user = _userRepository.GetUserByUsername(request.Username);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!string.IsNullOrEmpty(request.OldPassword))
            {
                if (!VerifyPasswordHash(request.OldPassword, user.PasswordHash, user.PasswordSalt))
                {
                    return BadRequest("Incorrect old password.");
                }
                CreatePasswordHash(request.NewPassword, out byte[] newPasswordHash, out byte[] newPasswordSalt);
                user.PasswordHash = newPasswordHash;
                user.PasswordSalt = newPasswordSalt;
            }
            _userRepository.UpdateUser(user);

            return Ok(user);
        }

        [HttpPut("editUser")]
        public async Task<ActionResult<User>> EditUser(EditRoleIdDTO request)
        {
            var user = _userRepository.GetUserByUsername(request.Username);
            
            if (user == null)
            {
                return NotFound("User not found.");
            }
            user.RoleId = request.RoleId;
            user.Email = request.Email;
            user.Status = request.Status;
            _userRepository.UpdateUser(user);


            return Ok(user);
        }


    }
}