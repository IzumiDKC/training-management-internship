using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Dtos;
using training_management_internship.Models;

namespace training_management_internship.ControllersAPI
{
    [Route("api/KhoaHoc")]
    [ApiController]
    public class KhoaHocAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KhoaHocAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/KhoaHoc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhoaHocDetailDto>>> GetAll()
        {
            var result = await _context.KhoaHocs
                .Include(k => k.ChuongTrinhDaoTao)
                .Include(k => k.Lops) 
                .Select(k => new KhoaHocDetailDto
                {
                    KhoaHocId = k.KhoaHocId,
                    TenKhoaHoc = k.TenKhoaHoc,
                    ChuongTrinhDaoTao = new ChuongTrinhShortDto
                    {
                        ChuongTrinhDaoTaoId = k.ChuongTrinhDaoTao.ChuongTrinhDaoTaoId,
                        TenChuongTrinh = k.ChuongTrinhDaoTao.TenChuongTrinh
                    },
                    Lops = k.Lops.Select(l => new LopDto
                    {
                        LopId = l.LopId,
                        TenLop = l.TenLop,
                        NgayBatDauDuKien = l.NgayBatDauDuKien,
                        NgayKetThucDuKien = l.NgayKetThucDuKien,
                        SoGio = l.SoGio,
                        SoGioQuyDoi = l.SoGioQuyDoi,
                        CoDanhSachHocVien = l.CoDanhSachHocVien
                    }).ToList()
                })
                    .ToListAsync();
            return Ok(result);
        }

        // GET: api/KhoaHoc/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KhoaHocDetailDto>> GetById(int id)
        {
            var khoaHoc = await _context.KhoaHocs
                .Include(k => k.ChuongTrinhDaoTao)
                .Where(k => k.KhoaHocId == id)
                .Select(k => new KhoaHocDetailDto
                {
                    KhoaHocId = k.KhoaHocId,
                    TenKhoaHoc = k.TenKhoaHoc,
                    ChuongTrinhDaoTao = new ChuongTrinhShortDto
                    {
                        ChuongTrinhDaoTaoId = k.ChuongTrinhDaoTao.ChuongTrinhDaoTaoId,
                        TenChuongTrinh = k.ChuongTrinhDaoTao.TenChuongTrinh
                    }
                })
                .FirstOrDefaultAsync();

            if (khoaHoc == null)
                return NotFound(new { message = $"Không tìm thấy khóa học với ID = {id}" });

            return Ok(khoaHoc);
        }

        // POST: api/KhoaHoc
        [HttpPost]
        public async Task<ActionResult<KhoaHocDto>> Create([FromBody] KhoaHoc model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Dữ liệu gửi lên không hợp lệ",
                    errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    )
                });
            }

            try
            {
                _context.KhoaHocs.Add(model);
                await _context.SaveChangesAsync();

                var dto = new KhoaHocDto
                {
                    KhoaHocId = model.KhoaHocId,
                    TenKhoaHoc = model.TenKhoaHoc
                };

                return CreatedAtAction(nameof(GetById), new { id = model.KhoaHocId }, dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo khóa học", error = ex.Message });
            }
        }

        // PUT: api/KhoaHoc/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] KhoaHoc model)
        {
            if (id != model.KhoaHocId)
                return BadRequest(new { message = "ID trong URL không khớp với dữ liệu gửi lên." });

            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Dữ liệu không hợp lệ",
                    errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    )
                });
            }

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KhoaHocExists(id))
                    return NotFound(new { message = $"Không tìm thấy khóa học với ID = {id}" });

                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật khóa học", error = ex.Message });
            }
        }

        // DELETE: api/KhoaHoc/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var khoaHoc = await _context.KhoaHocs.FindAsync(id);
            if (khoaHoc == null)
                return NotFound(new { message = $"Không tìm thấy khóa học với ID = {id}" });

            try
            {
                _context.KhoaHocs.Remove(khoaHoc);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Xóa khóa học thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa khóa học", error = ex.Message });
            }
        }

        private bool KhoaHocExists(int id)
        {
            return _context.KhoaHocs.Any(e => e.KhoaHocId == id);
        }
    }
}
