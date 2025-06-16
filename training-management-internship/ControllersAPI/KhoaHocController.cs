using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using training_management_internship.Dtos;

namespace training_management_internship.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KhoaHocController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KhoaHocController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.KhoaHocs
                .Include(k => k.ChuongTrinhDaoTao)
                .Select(k => new KhoaHocDto
                {
                    KhoaHocId = k.KhoaHocId,
                    TenKhoaHoc = k.TenKhoaHoc,
                    TenChuongTrinhDaoTao = k.ChuongTrinhDaoTao.TenChuongTrinh
                })
                .ToListAsync();

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var khoaHoc = await _context.KhoaHocs
                .Include(k => k.ChuongTrinhDaoTao)
                .FirstOrDefaultAsync(k => k.KhoaHocId == id);

            if (khoaHoc == null) return NotFound();
            return Ok(khoaHoc);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] KhoaHoc model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.KhoaHocs.Add(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] KhoaHoc model)
        {
            if (id != model.KhoaHocId) return BadRequest("ID mismatch");

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.KhoaHocs.FindAsync(id);
            if (entity == null) return NotFound();
            _context.KhoaHocs.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
