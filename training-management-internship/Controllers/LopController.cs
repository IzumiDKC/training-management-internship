using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;

namespace training_management_internship.Controllers
{
    public class LopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LopController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Lop
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Lops.Include(l => l.KhoaHoc).Include(l => l.LoaiLop);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Lop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lop = await _context.Lops
                .Include(l => l.KhoaHoc)
                .Include(l => l.LoaiLop)
                .Include(l => l.DanhSachHocViens)                    
                    .ThenInclude(ds => ds.HocVien)
                    .ThenInclude(hv => hv.User)
                .FirstOrDefaultAsync(m => m.LopId == id);

            if (lop == null)
            {
                return NotFound();
            }

            return View(lop);
        }


        // GET: Lop/Create
        public IActionResult Create()
        {
            ViewData["KhoaHocId"] = new SelectList(_context.KhoaHocs, "KhoaHocId", "TenKhoaHoc");
            ViewData["LoaiLopId"] = new SelectList(_context.LoaiLops, "LoaiLopId", "TenLoaiLop");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LopId,TenLop,NgayBatDauDuKien,NgayKetThucDuKien,SoGio,SoGioQuyDoi,CoDanhSachHocVien,KhoaHocId,LoaiLopId")] Lop lop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lop);
                await _context.SaveChangesAsync();

                if (lop.CoDanhSachHocVien)
                {
                    TempData["LopIdVuaTao"] = lop.LopId;
                    return RedirectToAction("ChonThemHocVien", "Lop"); // → đi đến bước popup
                }
                return RedirectToAction(nameof(Index));
            }
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($"ModelState Error - Field: {key}, Error: {error.ErrorMessage}");
                }
            }
            ViewData["KhoaHocId"] = new SelectList(_context.KhoaHocs, "KhoaHocId", "TenKhoaHoc", lop.KhoaHocId);
            ViewData["LoaiLopId"] = new SelectList(_context.LoaiLops, "LoaiLopId", "TenLoaiLop", lop.LoaiLopId);
            return View(lop);
        }

        [HttpGet]
        public IActionResult ChonThemHocVien()
        {
            if (TempData["LopIdVuaTao"] == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.LopId = TempData["LopIdVuaTao"];
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ChonHocVien(int lopId)
        {
            var hocViens = await _userManager.GetUsersInRoleAsync("HocVien");
            var model = hocViens.Select(u => new HocVienSelectorViewModel
            {
                UserId = u.Id,
                HoTen = u.HoTen,
                IsSelected = false
            }).ToList();

            ViewBag.LopId = lopId;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ThemHocVienVaoLop(List<HocVienSelectorViewModel> model, int lopId)
        {
            foreach (var item in model.Where(m => m.IsSelected))
            {
                var hocVien = await _context.HocViens.FirstOrDefaultAsync(h => h.UserId == item.UserId);
                if (hocVien != null)
                {
                    var danhSach = new DanhSachHocVien
                    {
                        LopId = lopId,
                        HocVienId = hocVien.HocVienId
                    };
                    _context.DanhSachHocViens.Add(danhSach);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Lop", new { id = lopId });
        }

        // GET: Lop/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lop = await _context.Lops.FindAsync(id);
            if (lop == null)
            {
                return NotFound();
            }
            ViewData["KhoaHocId"] = new SelectList(_context.KhoaHocs, "KhoaHocId", "TenKhoaHoc", lop.KhoaHocId);
            ViewData["LoaiLopId"] = new SelectList(_context.LoaiLops, "LoaiLopId", "TenLoaiLop", lop.LoaiLopId);
            return View(lop);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LopId,TenLop,NgayBatDauDuKien,NgayKetThucDuKien,SoGio,SoGioQuyDoi,CoDanhSachHocVien,KhoaHocId,LoaiLopId")] Lop lop)
        {
            if (id != lop.LopId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LopExists(lop.LopId))
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
            ViewData["KhoaHocId"] = new SelectList(_context.KhoaHocs, "KhoaHocId", "TenKhoaHoc", lop.KhoaHocId);
            ViewData["LoaiLopId"] = new SelectList(_context.LoaiLops, "LoaiLopId", "TenLoaiLop", lop.LoaiLopId);
            return View(lop);
        }

        // GET: Lop/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lop = await _context.Lops
                .Include(l => l.KhoaHoc)
                .Include(l => l.LoaiLop)
                .FirstOrDefaultAsync(m => m.LopId == id);
            if (lop == null)
            {
                return NotFound();
            }

            return View(lop);
        }

        // POST: Lop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lop = await _context.Lops.FindAsync(id);
            if (lop != null)
            {
                _context.Lops.Remove(lop);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LopExists(int id)
        {
            return _context.Lops.Any(e => e.LopId == id);
        }
    }
}
