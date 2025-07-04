using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using training_management_internship.Models;
using training_management_internship.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace training_management_internship.ControllersAPI
{
    [Route("api/account")]
    [ApiController]
    public class AccountAPIController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public AccountAPIController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_context.Users.Any(u => u.SoCanCuoc == model.SoCanCuoc))
            {
                return BadRequest(new { error = "Số căn cước này đã tồn tại trong hệ thống." });
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                HoTen = model.HoTen,
                SoCanCuoc = model.SoCanCuoc,
                EmailConfirmed = false 
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var role = model.Role ?? "HocVien";

            // Tạo role nếu chưa có
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            await _userManager.AddToRoleAsync(user, role);

            if (role == "HocVien")
            {
                _context.HocViens.Add(new HocVien { UserId = user.Id });
            }
            else if (role == "GiangVien")
            {
                _context.GiangViens.Add(new GiangVien { UserId = user.Id });
            }

            await _context.SaveChangesAsync();

            // link xác nhận email
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = $"https://localhost:7247/api/account/confirm-email?userId={user.Id}&code={code}";

            await _emailSender.SendEmailAsync(model.Email, "Xác nhận đăng ký",
                $"Vui lòng xác nhận tài khoản bằng cách nhấn vào link sau: <a href='{callbackUrl}'>Xác nhận</a>");

            return Ok(new { message = "Đăng ký thành công. Vui lòng kiểm tra email để xác nhận tài khoản." });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var token = GenerateJwtToken(user); 

                // AspNetUserTokens
                var existingToken = await _context.Set<IdentityUserToken<string>>()
                                                   .FirstOrDefaultAsync(t => t.UserId == user.Id && t.LoginProvider == "JWT" && t.Name == "access_token");

                if (existingToken != null)
                {
                    existingToken.Value = token;  // Cập nhật token
                    _context.Update(existingToken);
                }
                else
                {
                    var userToken = new IdentityUserToken<string>
                    {
                        UserId = user.Id,
                        LoginProvider = "JWT",
                        Name = "access_token",
                        Value = token
                    };
                    _context.Add(userToken);
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Đăng nhập thành công",
                    token
                });
            }

            return Unauthorized(new { error = "Tài khoản hoặc mật khẩu không đúng" });
        }


        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
    };

            var roles = _userManager.GetRolesAsync(user).Result;
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtConfig = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }




        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); 

            var user = await _userManager.GetUserAsync(User); 
            if (user != null)
            {
                var token = await _context.Set<IdentityUserToken<string>>()
                    .FirstOrDefaultAsync(t => t.UserId == user.Id && t.LoginProvider == "JWT" && t.Name == "access_token");

                if (token != null)
                {
                    _context.Set<IdentityUserToken<string>>().Remove(token); 
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new { message = "Đã đăng xuất thành công" });
        }


        // GET: api/account/me
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = await _context.Set<IdentityUserToken<string>>()
                                        .FirstOrDefaultAsync(t => t.UserId == user.Id && t.LoginProvider == "JWT" && t.Name == "access_token");

            if (token == null)
            {
                return Unauthorized(new { error = "Không tìm thấy token" });
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.HoTen,
                user.SoCanCuoc,
                Roles = roles,
             //   Token = token.Value 
            });
        }


        [Authorize]
        [HttpGet("GetUserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            Console.WriteLine("Received Token: " + token);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(new { message = "Token không hợp lệ!" });
            }

            return Ok(new { message = "Token hợp lệ!", userId = user.Id });
        }

        [Authorize]  
        [HttpGet("check-token")]
        public IActionResult CheckToken()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Token không hợp lệ!" });
            }

            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    return Unauthorized(new { message = "Token đã hết hạn!" });
                }

                return Ok(new { message = "Token hợp lệ!" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = "Token không hợp lệ!" });
            }
        }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return BadRequest(new { error = "Thiếu thông tin xác nhận." });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { error = $"Không tìm thấy người dùng với ID: {userId}" });

            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

            if (result.Succeeded)
                return Ok(new { message = "Xác nhận email thành công." });

            return BadRequest(new { error = "Xác nhận thất bại." });
        }

    }
}
