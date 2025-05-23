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
    public class DanhGiaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DanhGiaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DanhGias
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DanhGias.Include(d => d.DangKyKhoaHoc);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DanhGias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhGia = await _context.DanhGias
                .Include(d => d.DangKyKhoaHoc)
                .FirstOrDefaultAsync(m => m.DanhGiaId == id);
            if (danhGia == null)
            {
                return NotFound();
            }

            return View(danhGia);
        }

        // GET: DanhGias/Create
        public IActionResult Create()
        {
            ViewData["DangKyKhoaHocId"] = new SelectList(_context.DangKyKhoaHocs, "DangKyKhoaHocId", "DangKyKhoaHocId");
            return View();
        }

        // POST: DanhGias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DanhGiaId,DangKyKhoaHocId,NoiDung,Diem,NgayDanhGia")] DanhGia danhGia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhGia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DangKyKhoaHocId"] = new SelectList(_context.DangKyKhoaHocs, "DangKyKhoaHocId", "DangKyKhoaHocId", danhGia.DangKyKhoaHocId);
            return View(danhGia);
        }

        // GET: DanhGias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhGia = await _context.DanhGias.FindAsync(id);
            if (danhGia == null)
            {
                return NotFound();
            }
            ViewData["DangKyKhoaHocId"] = new SelectList(_context.DangKyKhoaHocs, "DangKyKhoaHocId", "DangKyKhoaHocId", danhGia.DangKyKhoaHocId);
            return View(danhGia);
        }

        // POST: DanhGias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DanhGiaId,DangKyKhoaHocId,NoiDung,Diem,NgayDanhGia")] DanhGia danhGia)
        {
            if (id != danhGia.DanhGiaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhGia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhGiaExists(danhGia.DanhGiaId))
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
            ViewData["DangKyKhoaHocId"] = new SelectList(_context.DangKyKhoaHocs, "DangKyKhoaHocId", "DangKyKhoaHocId", danhGia.DangKyKhoaHocId);
            return View(danhGia);
        }

        // GET: DanhGias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhGia = await _context.DanhGias
                .Include(d => d.DangKyKhoaHoc)
                .FirstOrDefaultAsync(m => m.DanhGiaId == id);
            if (danhGia == null)
            {
                return NotFound();
            }

            return View(danhGia);
        }

        // POST: DanhGias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhGia = await _context.DanhGias.FindAsync(id);
            if (danhGia != null)
            {
                _context.DanhGias.Remove(danhGia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanhGiaExists(int id)
        {
            return _context.DanhGias.Any(e => e.DanhGiaId == id);
        }
    }
}
