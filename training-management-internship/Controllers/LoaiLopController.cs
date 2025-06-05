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
    public class LoaiLopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoaiLopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LoaiLop
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoaiLops.ToListAsync());
        }

        // GET: LoaiLop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiLop = await _context.LoaiLops
                .FirstOrDefaultAsync(m => m.LoaiLopId == id);
            if (loaiLop == null)
            {
                return NotFound();
            }

            return View(loaiLop);
        }

        // GET: LoaiLop/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoaiLop loaiLop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiLop);
                await _context.SaveChangesAsync();
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

            return View(loaiLop);
        }



        // GET: LoaiLop/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiLop = await _context.LoaiLops.FindAsync(id);
            if (loaiLop == null)
            {
                return NotFound();
            }
            return View(loaiLop);
        }

        // POST: LoaiLop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LoaiLopId,TenLoaiLop")] LoaiLop loaiLop)
        {
            if (id != loaiLop.LoaiLopId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiLop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiLopExists(loaiLop.LoaiLopId))
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
            return View(loaiLop);
        }

        // GET: LoaiLop/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiLop = await _context.LoaiLops
                .FirstOrDefaultAsync(m => m.LoaiLopId == id);
            if (loaiLop == null)
            {
                return NotFound();
            }

            return View(loaiLop);
        }

        // POST: LoaiLop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiLop = await _context.LoaiLops.FindAsync(id);
            if (loaiLop != null)
            {
                _context.LoaiLops.Remove(loaiLop);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiLopExists(int id)
        {
            return _context.LoaiLops.Any(e => e.LoaiLopId == id);
        }
    }
}
