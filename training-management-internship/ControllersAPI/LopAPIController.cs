using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace training_management_internship.Controllers
{
    [Route("api/Lop")]
    [ApiController]
    public class LopAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LopAPIController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Lop
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LopDto>>> GetLops()
        {
            var lops = await _context.Lops
                .Include(l => l.KhoaHoc)
                .Include(l => l.LoaiLop)
                .Select(l => new LopDto
                {
                    LopId = l.LopId,
                    TenLop = l.TenLop,
                    NgayBatDauDuKien = l.NgayBatDauDuKien,
                    NgayKetThucDuKien = l.NgayKetThucDuKien,
                    SoGio = l.SoGio,
                    SoGioQuyDoi = l.SoGioQuyDoi,
                    CoDanhSachHocVien = l.CoDanhSachHocVien,
                    KhoaHocId = l.KhoaHocId,
                    KhoaHocName = l.KhoaHoc.TenKhoaHoc, // Lấy tên khóa học
                    LoaiLopId = l.LoaiLopId,
                    LoaiLopName = l.LoaiLop.TenLoaiLop, // Lấy tên loại lớp
                    DanhSachHocVienIds = l.DanhSachHocViens.Select(dshv => dshv.HocVienId).ToList() // Lấy danh sách ID học viên
                })
                .ToListAsync();

            return Ok(lops);
        }

        // GET: api/Lop/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LopDto>> GetLop(int id)
        {
            var lop = await _context.Lops
                .Include(l => l.KhoaHoc)
                .Include(l => l.LoaiLop)
                .Include(l => l.DanhSachHocViens)
                    .ThenInclude(ds => ds.HocVien)
                .Where(l => l.LopId == id)
                .Select(l => new LopDto
                {
                    LopId = l.LopId,
                    TenLop = l.TenLop,
                    NgayBatDauDuKien = l.NgayBatDauDuKien,
                    NgayKetThucDuKien = l.NgayKetThucDuKien,
                    SoGio = l.SoGio,
                    SoGioQuyDoi = l.SoGioQuyDoi,
                    CoDanhSachHocVien = l.CoDanhSachHocVien,
                    KhoaHocId = l.KhoaHocId,
                    KhoaHocName = l.KhoaHoc.TenKhoaHoc,
                    LoaiLopId = l.LoaiLopId,
                    LoaiLopName = l.LoaiLop.TenLoaiLop,
                    DanhSachHocVienIds = l.DanhSachHocViens.Select(dshv => dshv.HocVienId).ToList()
                })
                .FirstOrDefaultAsync();

            if (lop == null)
            {
                return NotFound();
            }

            return Ok(lop);
        }

        // POST: api/Lop
        [HttpPost]
        public async Task<ActionResult<Lop>> PostLop([FromBody] Lop lop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Lops.Add(lop);
            await _context.SaveChangesAsync();

            if (lop.CoDanhSachHocVien)
            {
                return Ok(new { message = "Lớp đã được tạo và chuyển đến trang chọn học viên." });
            }

            return CreatedAtAction(nameof(GetLop), new { id = lop.LopId }, lop);
        }

        // PUT: api/Lop/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLop(int id, [FromBody] Lop lop)
        {
            if (id != lop.LopId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Entry(lop).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LopExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Lop/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLop(int id)
        {
            var lop = await _context.Lops.FindAsync(id);
            if (lop == null)
            {
                return NotFound();
            }

            _context.Lops.Remove(lop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Lop/ChonHocVien/5
        [HttpGet("ChonHocVien/{lopId}")]
        public async Task<ActionResult<IEnumerable<HocVienSelectorViewModel>>> ChonHocVien(int lopId)
        {
            var hocViens = await _userManager.GetUsersInRoleAsync("HocVien");
            var model = hocViens.Select(u => new HocVienSelectorViewModel
            {
                UserId = u.Id,
                HoTen = u.HoTen,
                IsSelected = false
            }).ToList();

            return Ok(model);
        }

        // POST: api/Lop/ThemHocVienVaoLop
        [HttpPost("ThemHocVienVaoLop")]
        public async Task<IActionResult> ThemHocVienVaoLop([FromBody] List<HocVienSelectorViewModel> model, int lopId)
        {
            foreach (var item in model.Where(m => m.IsSelected))
            {
                var hocVien = await _context.HocViens.FirstOrDefaultAsync(h => h.UserId == item.UserId);
                if (hocVien != null)
                {
                    var danhSach = new DanhSachHocVien
                    {
                        LopId = lopId,
                        HocVienId = hocVien.HocVienId
                    };
                    _context.DanhSachHocViens.Add(danhSach);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Học viên đã được thêm vào lớp." });
        }

        private bool LopExists(int id)
        {
            return _context.Lops.Any(e => e.LopId == id);
        }
    }
}
