using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Dtos;
using training_management_internship.Models;

namespace training_management_internship.ControllersAPI
{
    [Route("api/Admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminAPIController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminAPIController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet("Users")]
        public async Task<ActionResult> GetPagedUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var totalUsers = await _context.Users.CountAsync();

            var users = await _context.Users
                .OrderBy(u => u.HoTen)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new List<UserWithRoleDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "Khac";

                result.Add(new UserWithRoleDto
                {
                    UserId = user.Id,
                    HoTen = user.HoTen,
                    Email = user.Email,
                    SoCanCuoc = user.SoCanCuoc,
                    Role = role
                });
            }

            var response = new
            {
                totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize),
                currentPage = page,
                users = result
            };

            return Ok(response);
        }


        [HttpPost("ChangeRole/{userId}")]
        public async Task<IActionResult> ChangeRole(string userId)
        {
            var user = await _userManager.Users
                .Include(u => u.HocVien)
                .Include(u => u.GiangVien)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            var currentRole = currentRoles.FirstOrDefault();

            string newRole = currentRole == "HocVien" ? "GiangVien" : "HocVien";

            if (!await _roleManager.RoleExistsAsync(newRole))
                await _roleManager.CreateAsync(new IdentityRole(newRole));

            await _userManager.RemoveFromRoleAsync(user, currentRole);
            await _userManager.AddToRoleAsync(user, newRole);

            if (currentRole == "HocVien" && user.HocVien != null)
                _context.HocViens.Remove(user.HocVien);
            else if (currentRole == "GiangVien" && user.GiangVien != null)
                _context.GiangViens.Remove(user.GiangVien);

            if (newRole == "HocVien")
                _context.HocViens.Add(new HocVien { UserId = user.Id });
            else if (newRole == "GiangVien")
                _context.GiangViens.Add(new GiangVien { UserId = user.Id });

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã chuyển {user.HoTen} sang vai trò {newRole}" });
        }

    }
}
