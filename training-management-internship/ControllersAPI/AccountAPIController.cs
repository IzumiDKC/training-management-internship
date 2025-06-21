using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using training_management_internship.Models;
using training_management_internship.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace training_management_internship.ControllersAPI
{
    [Route("api/account")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public AccountApiController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
            _roleManager = roleManager;
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


        // POST: api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return Ok(new { message = "Đăng nhập thành công" });
            }

            return Unauthorized(new { error = "Tài khoản hoặc mật khẩu không đúng" });
        }

        // POST: api/account/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Đăng xuất thành công" });
        }

        // GET: api/account/me
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            Console.WriteLine($" Identity.Name: {User.Identity?.Name}");
            Console.WriteLine($" IsAuthenticated: {User.Identity?.IsAuthenticated}");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Console.WriteLine("❌ Không tìm thấy user từ claims.");
                return Unauthorized();
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.HoTen,
                user.SoCanCuoc,
                Roles = roles
            });
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
