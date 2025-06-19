using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;

namespace training_management_internship.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new AdminDashboard
            {
                ChuongTrinhDaoTaos = await _context.ChuongTrinhDaoTaos
                    .OrderByDescending(c => c.ChuongTrinhDaoTaoId)
                    .Take(5).ToListAsync(),
                KhoaHocs = await _context.KhoaHocs
                    .Include(k => k.ChuongTrinhDaoTao)
                    .OrderByDescending(k => k.KhoaHocId)
                    .Take(5).ToListAsync(),
                Lops = await _context.Lops
                    .Include(l => l.KhoaHoc)
                    .Include(l => l.LoaiLop)
                    .OrderByDescending(l => l.LopId)
                    .Take(5).ToListAsync(),
                LoaiLops = await _context.LoaiLops
                    .OrderByDescending(l => l.LoaiLopId)
                    .Take(5).ToListAsync(),
                GiangViens = await _context.GiangViens
                    .OrderByDescending(g => g.GiangVienId)
                    .Take(5).ToListAsync(),
                Users = await _context.Users
                    .OrderByDescending(u => u.Id)
                    .Take(5).ToListAsync()
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Users(string roleFilter = "", string searchName = "")
        {
            var usersQuery = _context.Users
                .Include(u => u.HocVien)
                .Include(u => u.GiangVien)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
            {
                usersQuery = usersQuery.Where(u => u.HoTen.Contains(searchName));
            }

            var users = await usersQuery.ToListAsync();

            var userRoles = new Dictionary<string, string>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "Khac";
                userRoles[user.Id] = role;
            }

            if (!string.IsNullOrEmpty(roleFilter))
            {
                users = users.Where(u =>
                    userRoles.TryGetValue(u.Id, out var r) &&
                    string.Equals(r, roleFilter, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            ViewData["UserRoles"] = userRoles;
            ViewData["CurrentRoleFilter"] = roleFilter;
            ViewData["CurrentSearchName"] = searchName;

            return View(users);
        }



        [HttpGet]
        public async Task<IActionResult> ChangeRole(string userId)
        {
            var user = await _userManager.Users
                .Include(u => u.HocVien)
                .Include(u => u.GiangVien)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            var currentRole = currentRoles.FirstOrDefault();

            ViewData["CurrentRole"] = currentRole;
            ViewData["UserId"] = userId;
            ViewData["UserName"] = user.HoTen;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRoleConfirmed(string userId)
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
            {
                _context.HocViens.Remove(user.HocVien);
            }
            else if (currentRole == "GiangVien" && user.GiangVien != null)
            {
                _context.GiangViens.Remove(user.GiangVien);
            }

            if (newRole == "HocVien")
            {
                var hv = new HocVien { UserId = user.Id };
                _context.HocViens.Add(hv);
            }
            else if (newRole == "GiangVien")
            {
                var gv = new GiangVien { UserId = user.Id };
                _context.GiangViens.Add(gv);
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = $"✅ Đã chuyển {user.HoTen} sang vai trò {newRole}.";
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAll()
        {
            var giangViens = await _context.GiangViens.ToListAsync();
            _context.GiangViens.RemoveRange(giangViens);

            var hocViens = await _context.HocViens.ToListAsync();
            _context.HocViens.RemoveRange(hocViens);

            var users = await _context.Users.ToListAsync();
            _context.Users.RemoveRange(users);

            await _context.SaveChangesAsync();
            TempData["Message"] = "🗑️ Đã xóa tất cả user thành công!";
            return RedirectToAction(nameof(Users));
        }
    }
}
