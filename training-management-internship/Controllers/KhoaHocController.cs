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
    public class KhoaHocController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KhoaHocController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KhoaHocs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.KhoaHocs.Include(k => k.ChuongTrinhDaoTao);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: KhoaHocs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khoaHoc = await _context.KhoaHocs
                .Include(k => k.ChuongTrinhDaoTao)
                .FirstOrDefaultAsync(m => m.KhoaHocId == id);
            if (khoaHoc == null)
            {
                return NotFound();
            }

            return View(khoaHoc);
        }

        // GET: KhoaHocs/Create
        public IActionResult Create()
        {
            ViewData["ChuongTrinhDaoTaoId"] = new SelectList(_context.ChuongTrinhDaoTaos, "ChuongTrinhDaoTaoId", "ChuongTrinhDaoTaoId");
            return View();
        }

        // POST: KhoaHocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KhoaHocId,TenKhoaHoc,ChuongTrinhDaoTaoId")] KhoaHoc khoaHoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khoaHoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Log lỗi ra Console
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($"ModelState Error - Field: {key}, Error: {error.ErrorMessage}");
                }
            }

            ViewData["ChuongTrinhDaoTaoId"] = new SelectList(_context.ChuongTrinhDaoTaos, "ChuongTrinhDaoTaoId", "ChuongTrinhDaoTaoId", khoaHoc.ChuongTrinhDaoTaoId);
            return View(khoaHoc);
        }


        // GET: KhoaHocs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khoaHoc = await _context.KhoaHocs.FindAsync(id);
            if (khoaHoc == null)
            {
                return NotFound();
            }
            ViewData["ChuongTrinhDaoTaoId"] = new SelectList(_context.ChuongTrinhDaoTaos, "ChuongTrinhDaoTaoId", "ChuongTrinhDaoTaoId", khoaHoc.ChuongTrinhDaoTaoId);
            return View(khoaHoc);
        }

        // POST: KhoaHocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KhoaHocId,TenKhoaHoc,ChuongTrinhDaoTaoId")] KhoaHoc khoaHoc)
        {
            if (id != khoaHoc.KhoaHocId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khoaHoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhoaHocExists(khoaHoc.KhoaHocId))
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
            ViewData["ChuongTrinhDaoTaoId"] = new SelectList(_context.ChuongTrinhDaoTaos, "ChuongTrinhDaoTaoId", "ChuongTrinhDaoTaoId", khoaHoc.ChuongTrinhDaoTaoId);
            return View(khoaHoc);
        }

        // GET: KhoaHocs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khoaHoc = await _context.KhoaHocs
                .Include(k => k.ChuongTrinhDaoTao)
                .FirstOrDefaultAsync(m => m.KhoaHocId == id);
            if (khoaHoc == null)
            {
                return NotFound();
            }

            return View(khoaHoc);
        }

        // POST: KhoaHocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khoaHoc = await _context.KhoaHocs.FindAsync(id);
            if (khoaHoc != null)
            {
                _context.KhoaHocs.Remove(khoaHoc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhoaHocExists(int id)
        {
            return _context.KhoaHocs.Any(e => e.KhoaHocId == id);
        }
    }
}
