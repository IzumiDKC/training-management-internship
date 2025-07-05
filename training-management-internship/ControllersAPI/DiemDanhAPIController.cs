using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using QRCoder;
using training_management_internship.Models;
using training_management_internship.Services;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace training_management_internship.ControllersAPI
{
    [ApiController]
    [Authorize] 
    [Route("api/DiemDanh")]
    public class DiemDanhAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DiemDanhAPIController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("GetDiemDanhByChiTietLopId/{lopId}/{chiTietLopId}")]
        public async Task<ActionResult<IEnumerable<DiemDanhDto>>> GetDiemDanhByChiTietLopId(int lopId, int chiTietLopId)
        {
            var chiTietLop = await _context.ChiTietLops
                .Where(c => c.ChiTietLopId == chiTietLopId && c.LopId == lopId)
                .FirstOrDefaultAsync();

            if (chiTietLop == null)
            {
                return NotFound(new { message = "Không tìm thấy chi tiết lớp hoặc lớp không đúng." });
            }

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

            return Ok(diemDanhs);  
        }



        [HttpPost("DiemDanhSubmit")]
        public async Task<ActionResult> DiemDanhSubmit([FromBody] DiemDanhDto diemDanhDto)
        {
            var chiTietLop = await _context.ChiTietLops.FirstOrDefaultAsync(c => c.ChiTietLopId == diemDanhDto.ChiTietLopId);
            if (chiTietLop == null)
            {
                return BadRequest("ChiTietLopId không hợp lệ.");
            }

            var hocVien = await _context.HocViens.FirstOrDefaultAsync(h => h.HocVienId == diemDanhDto.HocVienId);
            if (hocVien == null)
            {
                return BadRequest("HocVienId không hợp lệ.");
            }

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
                
                diemDanh.CheckIn = diemDanhDto.CheckIn ?? diemDanh.CheckIn;
                diemDanh.CheckOut = diemDanhDto.CheckOut ?? diemDanh.CheckOut;
                diemDanh.Note = diemDanhDto.Note;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Điểm danh thành công" });
        }


        [Authorize(Roles = "Admin, GiangVien")]
        [HttpPost("ResetAllCheckIn/{chiTietLopId}")]
        public async Task<IActionResult> ResetAllCheckIn([FromRoute] int chiTietLopId)
        {
            var list = await _context.DiemDanhs
                .Where(d => d.ChiTietLopId == chiTietLopId)
                .ToListAsync();

            foreach (var diemDanh in list)
            {
                diemDanh.CheckIn = TimeSpan.Zero;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã reset {list.Count} bản ghi Check-in" });
        }




        [Authorize(Roles = "Admin, GiangVien")]
        [HttpPost("ResetAllCheckOut/{chiTietLopId}")]
        public async Task<IActionResult> ResetAllCheckOut([FromRoute] int chiTietLopId)
        {
            var list = await _context.DiemDanhs
                .Where(d => d.ChiTietLopId == chiTietLopId)
                .ToListAsync();

            foreach (var diemDanh in list)
            {
                diemDanh.CheckOut = TimeSpan.Zero;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã reset {list.Count} bản ghi Check-out" });
        }



        [Authorize(Roles = "Admin, GiangVien")]
        [HttpGet("GenerateQRBase64")]
        public async Task<IActionResult> GenerateQRBase64(int chiTietLopId, string type)
        {
            type = type.ToLower();
            var qrType = type == "checkin" ? QRCodeTemp.QRCodeType.CheckIn : QRCodeTemp.QRCodeType.CheckOut;

            var expiredThreshold = DateTime.Now.AddSeconds(-120);
            var existing = await _context.QRCodeTemps
                .Where(q => q.ChiTietLopId == chiTietLopId && q.Type == qrType && q.CreatedAt >= expiredThreshold)
                .OrderByDescending(q => q.CreatedAt)
                .FirstOrDefaultAsync();

            Guid token;
            if (existing != null)
            {
                token = existing.Token;
            }
            else
            {
                token = Guid.NewGuid();
                var qrEntry = new QRCodeTemp
                {
                    Token = token,
                    CreatedAt = DateTime.Now,
                    ChiTietLopId = chiTietLopId,
                    Type = qrType
                };
                _context.QRCodeTemps.Add(qrEntry);
                await _context.SaveChangesAsync();
            }

            var frontendUrl = $"http://localhost:3000/qr-scan/{token}";

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(frontendUrl, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new Base64QRCode(qrCodeData);
            var base64Image = qrCode.GetGraphic(20);

            return Ok(new { image = $"data:image/png;base64,{base64Image}" });
        }

        [Authorize]
        [HttpGet("Scan")]
        public async Task<IActionResult> Scan(Guid token)
        {
            var qr = await _context.QRCodeTemps.FirstOrDefaultAsync(q => q.Token == token);
            if (qr == null || (DateTime.Now - qr.CreatedAt).TotalSeconds > 120)
            {
                return Unauthorized(new { success = false, message = "QR code đã hết hạn hoặc không hợp lệ." });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var hocVien = await _context.HocViens.Include(h => h.User)
                                                  .FirstOrDefaultAsync(h => h.UserId == user.Id);

            if (hocVien == null)
            {
                hocVien = new HocVien { UserId = user.Id };
                _context.HocViens.Add(hocVien);
                await _context.SaveChangesAsync();
            }

            var now = DateTime.Now;

            var diemDanh = await _context.DiemDanhs.FirstOrDefaultAsync(d =>
                d.ChiTietLopId == qr.ChiTietLopId &&
                d.HocVienId == hocVien.HocVienId &&
                d.NgayCheck.Date == now.Date);  // Kiểm tra theo ngày học và học viên

            if (diemDanh == null)
            {
                diemDanh = new DiemDanh
                {
                    ChiTietLopId = qr.ChiTietLopId,
                    HocVienId = hocVien.HocVienId,
                    NgayCheck = now.Date,
                    CheckIn = qr.Type == QRCodeTemp.QRCodeType.CheckIn ? now.TimeOfDay : default,
                    CheckOut = qr.Type == QRCodeTemp.QRCodeType.CheckOut ? now.TimeOfDay : default
                };
                _context.DiemDanhs.Add(diemDanh);
            }
            else
            {
                if (qr.Type == QRCodeTemp.QRCodeType.CheckIn)
                    diemDanh.CheckIn = now.TimeOfDay;
                else if (qr.Type == QRCodeTemp.QRCodeType.CheckOut)
                    diemDanh.CheckOut = now.TimeOfDay;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Điểm danh thành công",
                checkInTime = diemDanh.CheckIn.ToString(@"hh\:mm"),
                checkOutTime = diemDanh.CheckOut.ToString(@"hh\:mm"),
                date = diemDanh.NgayCheck.ToString("dd/MM/yyyy")
            });
        }
}
    }
