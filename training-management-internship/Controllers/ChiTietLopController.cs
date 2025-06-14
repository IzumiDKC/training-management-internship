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
    public class ChiTietLopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChiTietLopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChiTietLops
        public async Task<IActionResult> Index(int lopId)
        {
            var chiTietLops = await _context.ChiTietLops
                .Include(c => c.GiangVien)
                    .ThenInclude(gv => gv.User)
                .Include(c => c.Lop)
                .Where(c => c.LopId == lopId)
                .ToListAsync();

            var lop = await _context.Lops.FirstOrDefaultAsync(l => l.LopId == lopId);
            ViewBag.Lop = lop;

            return View(chiTietLops);
        }



        // GET: ChiTietLops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietLop = await _context.ChiTietLops
                .Include(c => c.GiangVien)
                .ThenInclude(gv => gv.User)
                .Include(c => c.Lop)
                .FirstOrDefaultAsync(m => m.ChiTietLopId == id);
            if (chiTietLop == null)
            {
                return NotFound();
            }

            return View(chiTietLop);
        }

        // GET: ChiTietLops/Create
        public IActionResult Create(int lopId)
        {
            ViewData["GiangVienId"] = new SelectList(
                _context.GiangViens.Include(g => g.User),
                "GiangVienId",
                "User.HoTen"
            );

            var chiTiet = new ChiTietLop
            {
                LopId = lopId,
                NgayHoc = DateTime.Today,
                ThoiGianBatDau = new TimeSpan(7, 0, 0), 
                ThoiGianKetThuc = new TimeSpan(11, 0, 0) 
            };

            return View(chiTiet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NgayHoc,ThoiGianBatDau,ThoiGianKetThuc,LopId,GiangVienId")] ChiTietLop chiTietLop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietLop);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { lopId = chiTietLop.LopId });
            }

            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($"Lỗi - Field: {key}, Error: {error.ErrorMessage}");
                }
            }

            ViewBag.LopId = chiTietLop.LopId;
            ViewData["GiangVienId"] = new SelectList(
                _context.GiangViens.Include(g => g.User),
                "GiangVienId",
                "User.HoTen",
                chiTietLop.GiangVienId
            );

            return View(chiTietLop);
        }


        // GET: ChiTietLops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietLop = await _context.ChiTietLops
                .Include(c => c.GiangVien)
                .ThenInclude(g => g.User)
                .Include(c => c.Lop)
                .FirstOrDefaultAsync(c => c.ChiTietLopId == id);

            if (chiTietLop == null)
            {
                return NotFound();
            }

            var giangViens = await _context.GiangViens
                .Include(g => g.User)
                .ToListAsync();

            ViewData["GiangVienId"] = new SelectList(giangViens, "GiangVienId", "User.HoTen", chiTietLop.GiangVienId);

            return View(chiTietLop); // load binding từ DB
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChiTietLopId,NgayHoc,ThoiGianBatDau,ThoiGianKetThuc,LopId,GiangVienId")] ChiTietLop chiTietLop)
        {
            if (id != chiTietLop.ChiTietLopId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietLop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietLopExists(chiTietLop.ChiTietLopId))
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
            ViewData["GiangVienId"] = new SelectList(_context.GiangViens, "GiangVienId", "GiangVienId", chiTietLop.GiangVienId);
            ViewData["LopId"] = new SelectList(_context.Lops, "LopId", "TenLop", chiTietLop.LopId);
            return View(chiTietLop);
        }

        // GET: ChiTietLops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietLop = await _context.ChiTietLops
                .Include(c => c.GiangVien)
                .ThenInclude(gv => gv.User)
                .Include(c => c.Lop)
                .FirstOrDefaultAsync(m => m.ChiTietLopId == id);

            if (chiTietLop == null)
            {
                return NotFound();
            }

            return View(chiTietLop);
        }

        // POST: ChiTietLops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int lopId)
        {
            var chiTietLop = await _context.ChiTietLops.FindAsync(id);
            if (chiTietLop != null)
            {
                _context.ChiTietLops.Remove(chiTietLop);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { lopId = lopId });
        }


        private bool ChiTietLopExists(int id)
        {
            return _context.ChiTietLops.Any(e => e.ChiTietLopId == id);
        }
    }
}
