using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;

namespace training_management_internship.Controllers
{
   // [Authorize]
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
                    .Take(5)
                    .ToListAsync(),
                KhoaHocs = await _context.KhoaHocs
                    .Include(k => k.ChuongTrinhDaoTao)
                    .OrderByDescending(k => k.KhoaHocId)
                    .Take(5)
                    .ToListAsync(),
                Lops = await _context.Lops
                    .Include(l => l.KhoaHoc)
                    .Include(l => l.LoaiLop)
                    .OrderByDescending(l => l.LopId)
                    .Take(5)
                    .ToListAsync(),
                LoaiLops = await _context.LoaiLops
                    .OrderByDescending(l => l.LoaiLopId)
                    .Take(5)
                    .ToListAsync(),
                GiangViens = await _context.GiangViens
                    .OrderByDescending(g => g.GiangVienId)
                    .Take(5)
                    .ToListAsync(),
                Users = await _context.Users
                    .OrderByDescending(u => u.Id)
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
        }
    }
}