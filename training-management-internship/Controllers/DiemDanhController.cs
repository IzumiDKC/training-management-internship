using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using QRCoder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using training_management_internship.Services;

namespace training_management_internship.Controllers
{
/*     [Authorize(Roles = "Admin, GiangVien")]
*/    public class DiemDanhController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DiemDanhController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> DiemDanh(int chiTietLopId)
        {
            var chiTietLop = await _context.ChiTietLops
                .Include(c => c.Lop)
                    .ThenInclude(l => l.DanhSachHocViens)
                        .ThenInclude(ds => ds.HocVien)
                            .ThenInclude(hv => hv.User)
                .FirstOrDefaultAsync(c => c.ChiTietLopId == chiTietLopId);

            if (chiTietLop == null)
                return NotFound();

            var diemDanhDaCo = await _context.DiemDanhs
                .Where(d => d.ChiTietLopId == chiTietLopId)
                .ToListAsync();

            ViewBag.ChiTietLop = chiTietLop;
            ViewBag.DiemDanhs = diemDanhDaCo;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DiemDanhSubmit(int chiTietLopId, int hocVienId, string type, string note)
        {
            var now = DateTime.Now;

            var chiTietLop = await _context.ChiTietLops.FirstOrDefaultAsync(c => c.ChiTietLopId == chiTietLopId);
            if (chiTietLop == null)
            {
                return BadRequest("ChiTietLopId không hợp lệ.");
            }

            var diemDanh = await _context.DiemDanhs
                .FirstOrDefaultAsync(d => d.ChiTietLopId == chiTietLopId &&
                                          d.HocVienId == hocVienId &&
                                          d.NgayCheck.Date == now.Date);

            if (diemDanh == null)
            {
                diemDanh = new DiemDanh
                {
                    ChiTietLopId = chiTietLopId,
                    HocVienId = hocVienId,
                    NgayCheck = now.Date,
                    CheckIn = type == "checkin" ? now.TimeOfDay : TimeSpan.Zero,
                    CheckOut = type == "checkout" ? now.TimeOfDay : TimeSpan.Zero,
                    Note = note
                };
                _context.DiemDanhs.Add(diemDanh);
            }
            else
            {
                if (type == "checkin") diemDanh.CheckIn = now.TimeOfDay;
                else if (type == "checkout") diemDanh.CheckOut = now.TimeOfDay;
                diemDanh.Note = note;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("DiemDanh", new { chiTietLopId });
        }

        [HttpGet]
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

            var url = Url.Action("Scan", "DiemDanh", new { token }, Request.Scheme);
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new Base64QRCode(qrCodeData);
            var base64Image = qrCode.GetGraphic(20);

            return Json(new { image = $"data:image/png;base64,{base64Image}" });
        }

        public async Task<IActionResult> Scan(Guid token)
        {
            var qr = await _context.QRCodeTemps.FirstOrDefaultAsync(q => q.Token == token);
            if (qr == null || (DateTime.Now - qr.CreatedAt).TotalSeconds > 120)
            {
                return Unauthorized("QR code đã hết hạn hoặc không hợp lệ.");
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
                d.NgayCheck.Date == now.Date);

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
                else
                    diemDanh.CheckOut = now.TimeOfDay;
            }

            await _context.SaveChangesAsync();

            return View("Success");
        }
    }
}
