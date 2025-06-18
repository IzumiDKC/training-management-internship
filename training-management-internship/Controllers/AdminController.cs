using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;

namespace training_management_internship.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
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

            if (!string.IsNullOrEmpty(roleFilter))
            {
                if (roleFilter == "HocVien")
                {
                    usersQuery = usersQuery.Where(u => u.HocVien != null);
                }
                else if (roleFilter == "GiangVien")
                {
                    usersQuery = usersQuery.Where(u => u.GiangVien != null);
                }
                else if (roleFilter == "Admin")
                {
                    usersQuery = usersQuery.Where(u => u.HocVien == null && u.GiangVien == null);
                }
            }


            if (!string.IsNullOrEmpty(searchName))
            {
                usersQuery = usersQuery.Where(u => u.HoTen.Contains(searchName));
            }

            ViewData["CurrentRoleFilter"] = roleFilter;
            ViewData["CurrentSearchName"] = searchName;

            var users = await usersQuery.ToListAsync();
            return View(users); 
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
