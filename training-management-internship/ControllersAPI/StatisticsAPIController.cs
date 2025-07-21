using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;

namespace training_management_internship.ControllersAPI
{
    [Route("api/ThongKe")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ThongKeAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ThongKeAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview()
        {
            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1); 

            var accountsCreatedThisWeek = await _context.Users
                .Where(u => u.CreatedAt >= startOfWeek)
                .CountAsync();

            var topLops = await _context.Lops
                .Include(l => l.DanhSachHocViens)
                .OrderByDescending(l => l.DanhSachHocViens.Count)
                .Take(5)
                .Select(l => new
                {
                    l.LopId,
                    TenLop = l.TenLop,
                    SoLuongHocVien = l.DanhSachHocViens.Count
                })
                .ToListAsync();

            var tongHocVien = await _context.HocViens.CountAsync();
            var tongGiangVien = await _context.GiangViens.CountAsync();
            var tongLop = await _context.Lops.CountAsync();

            var topKhoaHoc = await _context.DangKyKhoaHocs
                .GroupBy(d => d.KhoaHocId)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    KhoaHocId = g.Key,
                    TenKhoaHoc = _context.KhoaHocs.FirstOrDefault(k => k.KhoaHocId == g.Key).TenKhoaHoc,
                    SoLuongDangKy = g.Count()
                })
                .Take(5)
                .ToListAsync();

            var soLuotDangKy = await _context.DangKyKhoaHocs.CountAsync();

            var lopDaiNhat = await _context.Lops
                .Where(l => l.NgayBatDauDuKien != null && l.NgayKetThucDuKien != null)
                .OrderByDescending(l => EF.Functions.DateDiffDay(l.NgayBatDauDuKien, l.NgayKetThucDuKien))
                .Select(l => new
                {
                    TenLop = l.TenLop,
                    ThoiGianNgay = EF.Functions.DateDiffDay(l.NgayBatDauDuKien, l.NgayKetThucDuKien)
                })
                .FirstOrDefaultAsync();

            var lopNganNhat = await _context.Lops
                .Where(l => l.NgayBatDauDuKien != null && l.NgayKetThucDuKien != null)
                .OrderBy(l => EF.Functions.DateDiffDay(l.NgayBatDauDuKien, l.NgayKetThucDuKien))
                .Select(l => new
                {
                    TenLop = l.TenLop,
                    ThoiGianNgay = EF.Functions.DateDiffDay(l.NgayBatDauDuKien, l.NgayKetThucDuKien)
                })
                .FirstOrDefaultAsync();

            return Ok(new
            {
                accountsCreatedThisWeek,
                tongHocVien,
                tongGiangVien,
                tongLop,
                topLops,
                soLuotDangKy,
                topKhoaHoc,
                lopDaiNhat,
                lopNganNhat
            });
        }


        [HttpGet("HocVienMoiTheoThang")]
        public async Task<IActionResult> GetHocVienMoiTheoThang()
        {
            var currentYear = DateTime.Now.Year;

            var result = await _context.HocViens
                .Where(hv => hv.User.CreatedAt.Year == currentYear)
                .GroupBy(hv => hv.User.CreatedAt.Month)
                .Select(g => new
                {
                    Thang = g.Key,
                    SoLuong = g.Count()
                })
                .ToListAsync();

            return Ok(result.OrderBy(x => x.Thang));
        }

     
    }
}
