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
    public class ChuongTrinhDaoTaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChuongTrinhDaoTaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChuongTrinhDaoTao
        public async Task<IActionResult> Index()
        {
            var danhSach = await _context.ChuongTrinhDaoTaos
                .Include(ct => ct.KhoaHocs) 
                .ToListAsync();

            return View(danhSach);
        }


        // GET: ChuongTrinhDaoTao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chuongTrinhDaoTao = await _context.ChuongTrinhDaoTaos
                .FirstOrDefaultAsync(m => m.ChuongTrinhDaoTaoId == id);
            if (chuongTrinhDaoTao == null)
            {
                return NotFound();
            }

            return View(chuongTrinhDaoTao);
        }

        // GET: ChuongTrinhDaoTao/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChuongTrinhDaoTaoId,TenChuongTrinh,MoTa")] ChuongTrinhDaoTao chuongTrinhDaoTao)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(chuongTrinhDaoTao);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Message: {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"Inner: {ex.InnerException.Message}");
                }
            }
            else
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Field '{key}': {error.ErrorMessage}");
                    }
                }
            }

            return View(chuongTrinhDaoTao);
        }


        // GET: ChuongTrinhDaoTao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chuongTrinhDaoTao = await _context.ChuongTrinhDaoTaos.FindAsync(id);
            if (chuongTrinhDaoTao == null)
            {
                return NotFound();
            }
            return View(chuongTrinhDaoTao);
        }

        // POST: ChuongTrinhDaoTao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChuongTrinhDaoTaoId,TenChuongTrinh,MoTa")] ChuongTrinhDaoTao chuongTrinhDaoTao)
        {
            if (id != chuongTrinhDaoTao.ChuongTrinhDaoTaoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chuongTrinhDaoTao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChuongTrinhDaoTaoExists(chuongTrinhDaoTao.ChuongTrinhDaoTaoId))
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
            return View(chuongTrinhDaoTao);
        }

        // GET: ChuongTrinhDaoTao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chuongTrinhDaoTao = await _context.ChuongTrinhDaoTaos
                .FirstOrDefaultAsync(m => m.ChuongTrinhDaoTaoId == id);
            if (chuongTrinhDaoTao == null)
            {
                return NotFound();
            }

            return View(chuongTrinhDaoTao);
        }

        // POST: ChuongTrinhDaoTao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chuongTrinhDaoTao = await _context.ChuongTrinhDaoTaos.FindAsync(id);
            if (chuongTrinhDaoTao != null)
            {
                _context.ChuongTrinhDaoTaos.Remove(chuongTrinhDaoTao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChuongTrinhDaoTaoExists(int id)
        {
            return _context.ChuongTrinhDaoTaos.Any(e => e.ChuongTrinhDaoTaoId == id);
        }
    }
}
