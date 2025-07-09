using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using Microsoft.AspNetCore.Identity;
using training_management_internship.Dtos;

namespace training_management_internship.ControllersAPI
{
    [Route("api/DangKyKhoaHoc")]
    [ApiController]
    [Authorize]
    public class DangKyKhoaHocAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DangKyKhoaHocAPIController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.DangKyKhoaHocs
                .Include(d => d.HocVien)
                .Include(d => d.KhoaHoc)
                .Select(d => new DangKyKhoaHocDto
                {
                    DangKyKhoaHocId = d.DangKyKhoaHocId,
                    NgayDangKy = d.NgayDangKy,
                    TenHocVien = d.HocVien.User.HoTen,
                    TenKhoaHoc = d.KhoaHoc.TenKhoaHoc
                })
                .ToListAsync();

            return Ok(list);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.DangKyKhoaHocs
                .Include(d => d.HocVien)
                .Include(d => d.KhoaHoc)
                .FirstOrDefaultAsync(d => d.DangKyKhoaHocId == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DangKyRequestDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var hocVien = await _context.HocViens.FirstOrDefaultAsync(h => h.UserId == user.Id);
            if (hocVien == null)
                return BadRequest("Không tìm thấy học viên tương ứng");

            var lopExists = await _context.Lops.AnyAsync(l => l.LopId == dto.LopId);
            if (!lopExists)
                return BadRequest($"Lớp với ID {dto.LopId} không tồn tại");

            var model = new DangKyKhoaHoc
            {
                HocVienId = hocVien.HocVienId,
                KhoaHocId = dto.KhoaHocId,
                NgayDangKy = DateTime.Now
            };

            _context.DangKyKhoaHocs.Add(model);
            await _context.SaveChangesAsync();

            _context.DanhSachHocViens.Add(new DanhSachHocVien
            {
                LopId = dto.LopId,
                HocVienId = hocVien.HocVienId
            });

            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công" });
        }






        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] DangKyKhoaHoc model)
        {
            if (id != model.DangKyKhoaHocId)
                return BadRequest("ID không khớp");

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DangKyKhoaHocExists(id))
                    return NotFound();

                throw;
            }
        }

        // DELETE: api/dang-ky-khoa-hoc/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.DangKyKhoaHocs.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.DangKyKhoaHocs.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("LopByKhoaHoc")]
        public async Task<IActionResult> GetLopByKhoaHoc([FromQuery] int khoaHocId)
        {
            var lops = await _context.Lops
                .Where(l => l.KhoaHocId == khoaHocId)
                .Select(l => new { l.LopId, l.TenLop })
                .ToListAsync();

            return Ok(lops);
        }

        private bool DangKyKhoaHocExists(int id)
        {
            return _context.DangKyKhoaHocs.Any(e => e.DangKyKhoaHocId == id);
        }
    }
}
