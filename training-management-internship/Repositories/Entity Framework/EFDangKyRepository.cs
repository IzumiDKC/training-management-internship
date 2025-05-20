using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using training_management_internship.Repositories.Interfaces;

namespace training_management_internship.Repositories
{
    public class EFDangKyRepository : IDangKyRepository
    {
        private readonly ApplicationDbContext _context;

        public EFDangKyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DangKyKhoaHoc>> GetAllAsync()
        {
            return await _context.DangKyKhoaHocs
                .Include(dk => dk.HocVien)
                .Include(dk => dk.KhoaHoc)
                .ToListAsync();
        }

        public async Task<DangKyKhoaHoc?> GetByIdAsync(int id)
        {
            return await _context.DangKyKhoaHocs
                .Include(dk => dk.HocVien)
                .Include(dk => dk.KhoaHoc)
                .FirstOrDefaultAsync(dk => dk.DangKyKhoaHocId == id);
        }

        public async Task<IEnumerable<DangKyKhoaHoc>> GetByHocVienIdAsync(int hocVienId)
        {
            return await _context.DangKyKhoaHocs
                .Where(dk => dk.HocVienId == hocVienId)
                .Include(dk => dk.KhoaHoc)
                .ToListAsync();
        }

        public async Task<IEnumerable<DangKyKhoaHoc>> GetByKhoaHocIdAsync(int khoaHocId)
        {
            return await _context.DangKyKhoaHocs
                .Where(dk => dk.KhoaHocId == khoaHocId)
                .Include(dk => dk.HocVien)
                .ToListAsync();
        }

        public async Task AddAsync(DangKyKhoaHoc dangKy)
        {
            _context.DangKyKhoaHocs.Add(dangKy);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DangKyKhoaHoc dangKy)
        {
            _context.DangKyKhoaHocs.Update(dangKy);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dangKy = await _context.DangKyKhoaHocs.FindAsync(id);
            if (dangKy != null)
            {
                _context.DangKyKhoaHocs.Remove(dangKy);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DangKyKhoaHocs.AnyAsync(dk => dk.DangKyKhoaHocId == id);
        }
    }
}
