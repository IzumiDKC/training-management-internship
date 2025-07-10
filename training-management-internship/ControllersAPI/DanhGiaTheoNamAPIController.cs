using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Dtos;
using training_management_internship.Models;

namespace training_management_internship.ControllersAPI
{
    [Route("api/DanhGiaTheoNam")]
    [ApiController]
    public class DanhGiaTheoNamAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DanhGiaTheoNamAPIController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, GiangVien")]
        public async Task<IActionResult> Create([FromBody] DanhGiaTheoNamCreateDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var model = new DanhGiaTheoNam
            {
                HocVienId = dto.HocVienId,
                Nam = dto.Nam,
                LoaiDanhGia = dto.LoaiDanhGia,
                NoiDung = dto.NoiDung,
                NgayDanhGia = DateTime.Now,
                NguoiDanhGiaId = user.Id
            };

            _context.DanhGiaTheoNams.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đánh giá tổng kết năm đã được tạo." });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.DanhGiaTheoNams
                .Include(d => d.HocVien).ThenInclude(h => h.User)
                .Include(d => d.NguoiDanhGia)
                .Select(d => new DanhGiaTheoNamDto
                {
                    Nam = d.Nam,
                    HoTen = d.HocVien.User.HoTen,
                    SoCanCuoc = d.HocVien.User.SoCanCuoc,
                    LoaiDanhGia = d.LoaiDanhGia,
                    NoiDung = d.NoiDung,
                    NgayDanhGia = d.NgayDanhGia,
                    NguoiDanhGia = d.NguoiDanhGia.UserName
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
