using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Dtos;
using training_management_internship.Models;

namespace training_management_internship.ControllersAPI
{
    [Route("api/ChuongTrinhDaoTao")]
    [ApiController]
    public class ChuongTrinhDaoTaoAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChuongTrinhDaoTaoAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ChuongTrinhDaoTao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChuongTrinhDto>>> GetAll()
        {
            var result = await _context.ChuongTrinhDaoTaos
                .Include(ct => ct.KhoaHocs)
                .Select(ct => new ChuongTrinhDto
                {
                    ChuongTrinhDaoTaoId = ct.ChuongTrinhDaoTaoId,
                    TenChuongTrinh = ct.TenChuongTrinh,
                    MoTa = ct.MoTa,
                    KhoaHocs = ct.KhoaHocs.Select(kh => new KhoaHocDto
                    {
                        KhoaHocId = kh.KhoaHocId,
                        TenKhoaHoc = kh.TenKhoaHoc
                    }).ToList()
                })
                .ToListAsync();

            return Ok(result);
        }

        // GET: api/ChuongTrinhDaoTao/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ChuongTrinhDto>> GetById(int id)
        {
            var item = await _context.ChuongTrinhDaoTaos
                .Include(ct => ct.KhoaHocs)
                .Where(ct => ct.ChuongTrinhDaoTaoId == id)
                .Select(ct => new ChuongTrinhDto
                {
                    ChuongTrinhDaoTaoId = ct.ChuongTrinhDaoTaoId,
                    TenChuongTrinh = ct.TenChuongTrinh,
                    MoTa = ct.MoTa,
                    KhoaHocs = ct.KhoaHocs.Select(kh => new KhoaHocDto
                    {
                        KhoaHocId = kh.KhoaHocId,
                        TenKhoaHoc = kh.TenKhoaHoc
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST: api/ChuongTrinhDaoTao
        [HttpPost]
        [Authorize(Roles = "Admin, GiangVien")]
        public async Task<ActionResult<ChuongTrinhDto>> Create([FromBody] ChuongTrinhDaoTao model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.ChuongTrinhDaoTaos.Add(model);
            await _context.SaveChangesAsync();

            var dto = new ChuongTrinhDto
            {
                ChuongTrinhDaoTaoId = model.ChuongTrinhDaoTaoId,
                TenChuongTrinh = model.TenChuongTrinh,
                MoTa = model.MoTa,
                KhoaHocs = new List<KhoaHocDto>()
            };

            return CreatedAtAction(nameof(GetById), new { id = dto.ChuongTrinhDaoTaoId }, dto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, GiangVien")]
        public async Task<IActionResult> Update(int id, [FromBody] ChuongTrinhDaoTao model)
        {
            if (id != model.ChuongTrinhDaoTaoId)
                return BadRequest("ID không khớp");

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ChuongTrinhDaoTaos.Any(e => e.ChuongTrinhDaoTaoId == id))
                    return NotFound();

                throw;
            }
        }

        // DELETE: api/ChuongTrinhDaoTao/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.ChuongTrinhDaoTaos.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.ChuongTrinhDaoTaos.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
