using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;

namespace training_management_internship.Controllers
{
    public class DanhSachHocViensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DanhSachHocViensController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DanhSachHocViens.Include(d => d.HocVien).Include(d => d.Lop);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhSachHocVien = await _context.DanhSachHocViens
                .Include(d => d.HocVien)
                .Include(d => d.Lop)
                .FirstOrDefaultAsync(m => m.DanhSachHocVienId == id);
            if (danhSachHocVien == null)
            {
                return NotFound();
            }

            return View(danhSachHocVien);
        }

        public IActionResult Create()
        {
            ViewData["HocVienId"] = new SelectList(_context.HocViens, "HocVienId", "HocVienId");
            ViewData["LopId"] = new SelectList(_context.Lops, "LopId", "TenLop");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DanhSachHocVienId,LopId,HocVienId")] DanhSachHocVien danhSachHocVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhSachHocVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HocVienId"] = new SelectList(_context.HocViens, "HocVienId", "HocVienId", danhSachHocVien.HocVienId);
            ViewData["LopId"] = new SelectList(_context.Lops, "LopId", "TenLop", danhSachHocVien.LopId);
            return View(danhSachHocVien);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhSachHocVien = await _context.DanhSachHocViens.FindAsync(id);
            if (danhSachHocVien == null)
            {
                return NotFound();
            }
            ViewData["HocVienId"] = new SelectList(_context.HocViens, "HocVienId", "HocVienId", danhSachHocVien.HocVienId);
            ViewData["LopId"] = new SelectList(_context.Lops, "LopId", "TenLop", danhSachHocVien.LopId);
            return View(danhSachHocVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DanhSachHocVienId,LopId,HocVienId")] DanhSachHocVien danhSachHocVien)
        {
            if (id != danhSachHocVien.DanhSachHocVienId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhSachHocVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhSachHocVienExists(danhSachHocVien.DanhSachHocVienId))
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
            ViewData["HocVienId"] = new SelectList(_context.HocViens, "HocVienId", "HocVienId", danhSachHocVien.HocVienId);
            ViewData["LopId"] = new SelectList(_context.Lops, "LopId", "TenLop", danhSachHocVien.LopId);
            return View(danhSachHocVien);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhSachHocVien = await _context.DanhSachHocViens
                .Include(d => d.HocVien)
                .Include(d => d.Lop)
                .FirstOrDefaultAsync(m => m.DanhSachHocVienId == id);
            if (danhSachHocVien == null)
            {
                return NotFound();
            }

            return View(danhSachHocVien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhSachHocVien = await _context.DanhSachHocViens.FindAsync(id);
            if (danhSachHocVien != null)
            {
                _context.DanhSachHocViens.Remove(danhSachHocVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanhSachHocVienExists(int id)
        {
            return _context.DanhSachHocViens.Any(e => e.DanhSachHocVienId == id);
        }
    }
}
