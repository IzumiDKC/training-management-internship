using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;

namespace training_management_internship.Controllers
{
    [Route("api/ChiTietLop")]
    [ApiController]
    public class ChiTietLopAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChiTietLopAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ChiTietLopAPI/lop/5
        [HttpGet("lop/{lopId}")]
        public async Task<ActionResult<IEnumerable<ChiTietLopDto>>> GetByLop(int lopId)
        {
            var chiTietLops = await _context.ChiTietLops
                .Include(c => c.Lop)
                .Include(c => c.GiangVien)
                    .ThenInclude(gv => gv.User)
                .Where(c => c.LopId == lopId)
                .Select(c => new ChiTietLopDto
                {
                    ChiTietLopId = c.ChiTietLopId,
                    NgayHoc = c.NgayHoc,
                    ThoiGianBatDau = c.ThoiGianBatDau,
                    ThoiGianKetThuc = c.ThoiGianKetThuc,
                    LopId = c.LopId,
                    TenLop = c.Lop.TenLop,
                    GiangVienId = c.GiangVienId,
                    TenGiangVien = c.GiangVien != null ? c.GiangVien.User.HoTen : null
                })
                .ToListAsync();

            return Ok(chiTietLops);
        }

        // GET: api/ChiTietLopAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChiTietLopDto>> GetById(int id)
        {
            var ct = await _context.ChiTietLops
                .Include(c => c.Lop)
                .Include(c => c.GiangVien)
                    .ThenInclude(gv => gv.User)
                .Where(c => c.ChiTietLopId == id)
                .Select(c => new ChiTietLopDto
                {
                    ChiTietLopId = c.ChiTietLopId,
                    NgayHoc = c.NgayHoc,
                    ThoiGianBatDau = c.ThoiGianBatDau,
                    ThoiGianKetThuc = c.ThoiGianKetThuc,
                    LopId = c.LopId,
                    TenLop = c.Lop.TenLop,
                    GiangVienId = c.GiangVienId,
                    TenGiangVien = c.GiangVien != null ? c.GiangVien.User.HoTen : null
                })
                .FirstOrDefaultAsync();

            if (ct == null)
                return NotFound();

            return Ok(ct);
        }

        // POST: api/ChiTietLopAPI
        [HttpPost]
        public async Task<ActionResult<ChiTietLop>> Create([FromBody] ChiTietLop chiTietLop)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.ChiTietLops.Add(chiTietLop);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = chiTietLop.ChiTietLopId }, chiTietLop);
        }

        // PUT: api/ChiTietLopAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ChiTietLop chiTietLop)
        {
            if (id != chiTietLop.ChiTietLopId)
                return BadRequest();

            _context.Entry(chiTietLop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ChiTietLops.Any(e => e.ChiTietLopId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/ChiTietLopAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var chiTietLop = await _context.ChiTietLops.FindAsync(id);
            if (chiTietLop == null)
                return NotFound();

            _context.ChiTietLops.Remove(chiTietLop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/ChiTietLop/giangviens
        [HttpGet("giangvien")]
        public async Task<ActionResult<IEnumerable<object>>> GetGiangViens()
        {
            var giangViens = await _context.GiangViens
                .Include(g => g.User)
                .Select(g => new {
                    giangVienId = g.GiangVienId,
                    hoTen = g.User.HoTen
                })
                .ToListAsync();

            return Ok(giangViens);
        }


    }
}
