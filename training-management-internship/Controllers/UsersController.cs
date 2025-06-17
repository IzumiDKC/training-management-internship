using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;

namespace training_management_internship.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: UsersController
        public async Task<IActionResult> Index(string roleFilter = "", string searchName = "")
        {
            var usersQuery = _context.Users
                .Include(u => u.HocVien)
                .Include(u => u.GiangVien)
                .AsQueryable();

            if (!string.IsNullOrEmpty(roleFilter))
            {
                if (roleFilter == "HocVien")
                    usersQuery = usersQuery.Where(u => u.HocVien != null);
                else if (roleFilter == "GiangVien")
                    usersQuery = usersQuery.Where(u => u.GiangVien != null);
                else if (roleFilter == "Khac")
                    usersQuery = usersQuery.Where(u => u.HocVien == null && u.GiangVien == null);
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



        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
            return RedirectToAction(nameof(Index));
        }

    }
}
