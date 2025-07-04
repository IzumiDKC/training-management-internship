using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using training_management_internship.Dtos;

namespace training_management_internship.ControllersAPI
{
    [ApiController]
    [Route("api/DanhSachHocVien")]
    [Authorize(Roles = "Admin, GiangVien")]
    public class DanhSachHocVienAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DanhSachHocVienAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetByLopId/{lopId}")]
        public async Task<ActionResult<IEnumerable<DanhSachHocVienDto>>> GetByLopId(int lopId)
        {

            var danhSachHocViens = await _context.DanhSachHocViens
                .Where(d => d.LopId == lopId)
                .Include(d => d.HocVien)
                .Include(d => d.Lop)
                .Select(d => new DanhSachHocVienDto
                {
                    DanhSachHocVienId = d.DanhSachHocVienId,
                    LopId = d.LopId,
                    HocVienId = d.HocVienId,
                    HocVienName = d.HocVien.User.HoTen,
                    SoCanCuoc = d.HocVien.User.SoCanCuoc,
                    LopName = d.Lop.TenLop
                })
                .ToListAsync();

            if (danhSachHocViens == null || !danhSachHocViens.Any())
            {
                return NotFound("Không có học viên nào trong lớp này.");
            }

            return Ok(danhSachHocViens);
        }

        [HttpPost("AddHocVienToLop")]
        public async Task<ActionResult> AddHocVienToLop([FromBody] DanhSachHocVienDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var newDanhSachHocVien = new DanhSachHocVien
            {
                LopId = dto.LopId,
                HocVienId = dto.HocVienId
            };

            _context.DanhSachHocViens.Add(newDanhSachHocVien);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Học viên đã được thêm vào lớp thành công." });
        }

        [HttpPut("UpdateDanhSachHocVien/{id}")]
        public async Task<ActionResult> UpdateDanhSachHocVien(int id, [FromBody] DanhSachHocVienDto dto)
        {
            var existingDanhSachHocVien = await _context.DanhSachHocViens.FindAsync(id);
            if (existingDanhSachHocVien == null)
            {
                return NotFound("Không tìm thấy học viên trong lớp.");
            }

            existingDanhSachHocVien.LopId = dto.LopId;
            existingDanhSachHocVien.HocVienId = dto.HocVienId;

            _context.DanhSachHocViens.Update(existingDanhSachHocVien);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thông tin học viên đã được cập nhật thành công." });
        }

        [HttpDelete("DeleteDanhSachHocVien/{id}")]
        public async Task<ActionResult> DeleteDanhSachHocVien(int id)
        {
            var danhSachHocVien = await _context.DanhSachHocViens.FindAsync(id);
            if (danhSachHocVien == null)
            {
                return NotFound("Không tìm thấy học viên trong lớp.");
            }

            _context.DanhSachHocViens.Remove(danhSachHocVien);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Học viên đã được xóa khỏi lớp." });
        }
    }
}
