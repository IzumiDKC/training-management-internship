using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using training_management_internship.Models;
using Microsoft.EntityFrameworkCore;
namespace training_management_internship.ControllersAPI
{
    [ApiController]
    [Route("api/DiemDanh")]
    [Authorize(Roles = "Admin, GiangVien")]
    public class DiemDanhAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DiemDanhAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetDiemDanhByChiTietLopId/{chiTietLopId}")]
        public async Task<ActionResult<IEnumerable<DiemDanhDto>>> GetDiemDanhByChiTietLopId(int chiTietLopId)
        {
            var diemDanhs = await _context.DiemDanhs
                .Where(d => d.ChiTietLopId == chiTietLopId)
                .Include(d => d.HocVien)
                .Select(d => new DiemDanhDto
                {
                    DiemDanhId = d.DiemDanhId,
                    NgayCheck = d.NgayCheck.Date,  
                    CheckIn = d.CheckIn == TimeSpan.Zero ? (TimeSpan?)null : d.CheckIn, 
                    CheckOut = d.CheckOut == TimeSpan.Zero ? (TimeSpan?)null : d.CheckOut, 
                    HocVienId = d.HocVienId,
                    Note = d.Note,
                    HocVienName = d.HocVien.User.HoTen,
                    SoCanCuoc = d.HocVien.User.SoCanCuoc,
                    ChiTietLopId = d.ChiTietLopId  
                })
                .ToListAsync();

            if (diemDanhs == null)
            {
                return NotFound();
            }

            return Ok(diemDanhs);
        }

        [HttpPost("DiemDanhSubmit")]
        public async Task<ActionResult> DiemDanhSubmit([FromBody] DiemDanhDto diemDanhDto)
        {
            var diemDanh = await _context.DiemDanhs
                .FirstOrDefaultAsync(d => d.ChiTietLopId == diemDanhDto.ChiTietLopId &&
                                          d.HocVienId == diemDanhDto.HocVienId &&
                                          d.NgayCheck.Date == diemDanhDto.NgayCheck.Date);

            if (diemDanh == null)
            {
                diemDanh = new DiemDanh
                {
                    ChiTietLopId = diemDanhDto.ChiTietLopId,
                    HocVienId = diemDanhDto.HocVienId,
                    NgayCheck = diemDanhDto.NgayCheck,
                    CheckIn = diemDanhDto.CheckIn ?? TimeSpan.Zero,
                    CheckOut = diemDanhDto.CheckOut ?? TimeSpan.Zero, 
                    Note = diemDanhDto.Note
                };
                _context.DiemDanhs.Add(diemDanh);
            }
            else
            {
                diemDanh.CheckIn = diemDanhDto.CheckIn ?? TimeSpan.Zero;   
                diemDanh.CheckOut = diemDanhDto.CheckOut ?? TimeSpan.Zero;
                diemDanh.Note = diemDanhDto.Note;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Điểm danh thành công" });
        }

        [HttpPost("ResetCheckIn")]
        public async Task<ActionResult> ResetCheckIn([FromBody] DiemDanhDto diemDanhDto)
        {
            var diemDanh = await _context.DiemDanhs
                .FirstOrDefaultAsync(d => d.ChiTietLopId == diemDanhDto.ChiTietLopId &&
                                          d.HocVienId == diemDanhDto.HocVienId &&
                                          d.NgayCheck.Date == diemDanhDto.NgayCheck.Date);

            if (diemDanh != null)
            {
                diemDanh.CheckIn = TimeSpan.Zero;
                _context.Update(diemDanh);
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Reset Check-in thành công" });
        }


        [HttpPost("ResetAllCheckIn")]
        public async Task<ActionResult> ResetAllCheckIn([FromBody] int chiTietLopId)
        {
            var diemDanhs = await _context.DiemDanhs
                .Where(d => d.ChiTietLopId == chiTietLopId)
                .ToListAsync();

            if (diemDanhs == null || diemDanhs.Count == 0)
            {
                return NotFound("Không có điểm danh nào trong buổi học này.");
            }

            foreach (var diemDanh in diemDanhs)
            {
                diemDanh.CheckIn = TimeSpan.Zero;
            }

            _context.UpdateRange(diemDanhs);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Reset toàn bộ CheckIn thành công" });
        }


        [HttpPost("ResetCheckOut")]
        public async Task<ActionResult> ResetCheckOut([FromBody] DiemDanhDto diemDanhDto)
        {
            var diemDanh = await _context.DiemDanhs
                .FirstOrDefaultAsync(d => d.ChiTietLopId == diemDanhDto.ChiTietLopId &&
                                          d.HocVienId == diemDanhDto.HocVienId &&
                                          d.NgayCheck.Date == diemDanhDto.NgayCheck.Date);

            if (diemDanh != null)
            {
                diemDanh.CheckOut = TimeSpan.Zero;
                _context.Update(diemDanh);
                await _context.SaveChangesAsync();
            }
            return Ok(new { message = "Reset Check-out thành công" });
        }


        [HttpPost("ResetAllCheckOut")]
        public async Task<ActionResult> ResetAllCheckOut([FromBody] int chiTietLopId)
        {
            var diemDanhs = await _context.DiemDanhs
                .Where(d => d.ChiTietLopId == chiTietLopId)
                .ToListAsync();

            if (diemDanhs == null || diemDanhs.Count == 0)
            {
                return NotFound("Không có điểm danh nào trong buổi học này.");
            }

            foreach (var diemDanh in diemDanhs)
            {
                diemDanh.CheckOut = TimeSpan.Zero;  
            }
            _context.UpdateRange(diemDanhs);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Reset toàn bộ CheckOut thành công" });
        }

    }
}
