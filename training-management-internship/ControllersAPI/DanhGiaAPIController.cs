using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Dtos;
using training_management_internship.Models;

namespace training_management_internship.ControllersAPI
{
    [Route("api/DanhGia")]
    [ApiController]
    public class DanhGiaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DanhGiaAPIController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, GiangVien")]
        public async Task<IActionResult> Create([FromBody] DanhGiaCreateDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var danhGia = new DanhGia
            {
                HocVienId = dto.HocVienId,
                LopId = dto.LopId,
                LoaiDanhGia = dto.LoaiDanhGia,
                NoiDung = dto.NoiDung,
                NgayDanhGia = DateTime.Now,
                NguoiDanhGiaId = user.Id
            };

            _context.DanhGias.Add(danhGia);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đánh giá đã được tạo." });
        }

        [HttpGet("theo-nam")]
        [Authorize(Roles = "Admin, GiangVien")]
        public async Task<IActionResult> GetByYear(int nam)
        {
            var result = await _context.DanhGias
                .Where(d => d.NgayDanhGia.Year == nam)
                .Include(d => d.HocVien).ThenInclude(h => h.User)
                .Include(d => d.Lop)
                .GroupBy(d => d.HocVienId)
                .Select(g => new
                {
                    Nam = nam,
                    HoTen = g.First().HocVien.User.HoTen,
                    SoCanCuoc = g.First().HocVien.User.SoCanCuoc,
                    TenLop = g.First().Lop.TenLop,
                    DanhGias = g.Select(d => new DanhGiaChiTietDto
                    {
                        LoaiDanhGia = d.LoaiDanhGia,
                        NoiDung = d.NoiDung,
                        NgayDanhGia = d.NgayDanhGia
                    }).ToList()
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var danhGias = await _context.DanhGias
                .Include(d => d.HocVien).ThenInclude(h => h.User)
                .Include(d => d.Lop)
                .Include(d => d.NguoiDanhGia)
                .Select(d => new DanhGiaDto
                {
                    DanhGiaId = d.DanhGiaId,
                    HoTen = d.HocVien.User.HoTen,
                    SoCanCuoc = d.HocVien.User.SoCanCuoc,
                    TenLop = d.Lop.TenLop,
                    LoaiDanhGia = d.LoaiDanhGia,
                    NoiDung = d.NoiDung,
                    NgayDanhGia = d.NgayDanhGia,
                    NguoiDanhGia = d.NguoiDanhGia.UserName
                })
                .ToListAsync();

            return Ok(danhGias);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var danhGia = await _context.DanhGias.FindAsync(id);
            if (danhGia == null) return NotFound();

            _context.DanhGias.Remove(danhGia);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xoá đánh giá thành công." });
        }
    }
}