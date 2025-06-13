using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace training_management_internship.Controllers
{
    [Authorize]
    public class DangKyKhoaHocController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DangKyKhoaHocController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DangKyKhoaHoc
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DangKyKhoaHocs.Include(d => d.HocVien).Include(d => d.KhoaHoc);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["KhoaHocId"] = new SelectList(_context.KhoaHocs, "KhoaHocId", "TenKhoaHoc");
            ViewData["LopId"] = new SelectList(Enumerable.Empty<Lop>(), "LopId", "TenLop"); // empty initially
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KhoaHocId")] DangKyKhoaHoc dangKyKhoaHoc, int LopId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var hocVien = await _context.HocViens.FirstOrDefaultAsync(h => h.UserId == user.Id);
            if (hocVien == null)
            {
                ModelState.AddModelError("", "Không tìm thấy học viên.");
                return View();
            }

            dangKyKhoaHoc.HocVienId = hocVien.HocVienId;
            dangKyKhoaHoc.NgayDangKy = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.DangKyKhoaHocs.Add(dangKyKhoaHoc);
                await _context.SaveChangesAsync();

                // Thêm học viên vào lớp
                var dsHocVien = new DanhSachHocVien
                {
                    LopId = LopId,
                    HocVienId = hocVien.HocVienId
                };

                _context.DanhSachHocViens.Add(dsHocVien);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["KhoaHocId"] = new SelectList(_context.KhoaHocs, "KhoaHocId", "TenKhoaHoc", dangKyKhoaHoc.KhoaHocId);
            return View(dangKyKhoaHoc);
        }



        // AJAX: Trả danh sách lớp theo Khóa học
        [HttpGet]
        public async Task<JsonResult> GetLopByKhoaHoc(int khoaHocId)
        {
            var lops = await _context.Lops
                .Where(l => l.KhoaHocId == khoaHocId)
                .Select(l => new { l.LopId, l.TenLop })
                .ToListAsync();

            return Json(lops);
        }

        // GET: DangKyKhoaHoc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dangKyKhoaHoc = await _context.DangKyKhoaHocs
                .Include(d => d.HocVien)
                .Include(d => d.KhoaHoc)
                .FirstOrDefaultAsync(m => m.DangKyKhoaHocId == id);
            if (dangKyKhoaHoc == null)
            {
                return NotFound();
            }

            return View(dangKyKhoaHoc);
        }
        
        // GET: DangKyKhoaHoc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dangKyKhoaHoc = await _context.DangKyKhoaHocs.FindAsync(id);
            if (dangKyKhoaHoc == null)
            {
                return NotFound();
            }
            ViewData["HocVienId"] = new SelectList(_context.HocViens, "HocVienId", "HocVienId", dangKyKhoaHoc.HocVienId);
            ViewData["KhoaHocId"] = new SelectList(_context.KhoaHocs, "KhoaHocId", "TenKhoaHoc", dangKyKhoaHoc.KhoaHocId);
            return View(dangKyKhoaHoc);
        }

        // GET: DangKyKhoaHoc/Create


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DangKyKhoaHocId,HocVienId,KhoaHocId,NgayDangKy")] DangKyKhoaHoc dangKyKhoaHoc)
        {
            if (id != dangKyKhoaHoc.DangKyKhoaHocId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dangKyKhoaHoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DangKyKhoaHocExists(dangKyKhoaHoc.DangKyKhoaHocId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["HocVienId"] = new SelectList(_context.HocViens, "HocVienId", "HocVienId", dangKyKhoaHoc.HocVienId);
            ViewData["KhoaHocId"] = new SelectList(_context.KhoaHocs, "KhoaHocId", "TenKhoaHoc", dangKyKhoaHoc.KhoaHocId);
            return View(dangKyKhoaHoc);
        }

        // GET: DangKyKhoaHoc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dangKyKhoaHoc = await _context.DangKyKhoaHocs
                .Include(d => d.HocVien)
                .Include(d => d.KhoaHoc)
                .FirstOrDefaultAsync(m => m.DangKyKhoaHocId == id);
            if (dangKyKhoaHoc == null)
            {
                return NotFound();
            }

            return View(dangKyKhoaHoc);
        }

        // POST: DangKyKhoaHoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dangKyKhoaHoc = await _context.DangKyKhoaHocs.FindAsync(id);
            if (dangKyKhoaHoc != null)
            {
                _context.DangKyKhoaHocs.Remove(dangKyKhoaHoc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DangKyKhoaHocExists(int id)
        {
            return _context.DangKyKhoaHocs.Any(e => e.DangKyKhoaHocId == id);
        }
    }
}
