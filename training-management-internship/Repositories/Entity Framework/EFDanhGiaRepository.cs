using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using training_management_internship.Repositories.Interfaces;

namespace training_management_internship.Repositories
{
    public class EFDanhGiaRepository : IDanhGiaRepository
    {
        private readonly ApplicationDbContext _context;

        public EFDanhGiaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DanhGia>> GetAllAsync()
        {
            return await _context.DanhGias
                .Include(dg => dg.DangKyKhoaHoc)
                    .ThenInclude(dk => dk.HocVien)
                .Include(dg => dg.DangKyKhoaHoc)
                    .ThenInclude(dk => dk.KhoaHoc)
                .ToListAsync();
        }


        public async Task<DanhGia?> GetByIdAsync(int id)
        {
            return await _context.DanhGias
                .Include(dg => dg.DangKyKhoaHoc)
                    .ThenInclude(dk => dk.HocVien)
                .Include(dg => dg.DangKyKhoaHoc)
                    .ThenInclude(dk => dk.KhoaHoc)
                .FirstOrDefaultAsync(dg => dg.DanhGiaId == id);

        }

        public async Task AddAsync(DanhGia danhGia)
        {
            _context.DanhGias.Add(danhGia);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DanhGia danhGia)
        {
            _context.DanhGias.Update(danhGia);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var danhGia = await _context.DanhGias.FindAsync(id);
            if (danhGia != null)
            {
                _context.DanhGias.Remove(danhGia);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DanhGias.AnyAsync(dg => dg.DanhGiaId == id);
        }
    }
}
