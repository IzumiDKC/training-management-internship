using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Dtos;
using training_management_internship.Models;

namespace training_management_internship.ControllersAPI
{
    [Route("api/LoaiLop")]
    [ApiController]
    public class LoaiLopAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoaiLopAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LoaiLop
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiLopDto>>> GetAll()
        {
            var result = await _context.LoaiLops
                .Select(l => new LoaiLopDto
                {
                    LoaiLopId = l.LoaiLopId,
                    TenLoaiLop = l.TenLoaiLop
                })
                .ToListAsync();

            return Ok(result);
        }

        // GET: api/LoaiLop/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoaiLopDto>> GetById(int id)
        {
            var loaiLop = await _context.LoaiLops
                .Where(l => l.LoaiLopId == id)
                .Select(l => new LoaiLopDto
                {
                    LoaiLopId = l.LoaiLopId,
                    TenLoaiLop = l.TenLoaiLop
                })
                .FirstOrDefaultAsync();

            if (loaiLop == null)
                return NotFound(new { message = $"Không tìm thấy loại lớp với ID = {id}" });

            return Ok(loaiLop);
        }

        // POST: api/LoaiLop
        [HttpPost]
        public async Task<ActionResult<LoaiLopDto>> Create([FromBody] LoaiLopDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = new LoaiLop
            {
                TenLoaiLop = dto.TenLoaiLop
            };

            _context.LoaiLops.Add(model);
            await _context.SaveChangesAsync();

            dto.LoaiLopId = model.LoaiLopId;

            return CreatedAtAction(nameof(GetById), new { id = dto.LoaiLopId }, dto);
        }

        // PUT: api/LoaiLop/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LoaiLopDto dto)
        {
            if (id != dto.LoaiLopId)
                return BadRequest(new { message = "ID không khớp." });

            var entity = await _context.LoaiLops.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = $"Không tìm thấy loại lớp với ID = {id}" });

            entity.TenLoaiLop = dto.TenLoaiLop;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật thành công." });
        }

        // DELETE: api/LoaiLop/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.LoaiLops.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = $"Không tìm thấy loại lớp với ID = {id}" });

            _context.LoaiLops.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công." });
        }
    }
}
