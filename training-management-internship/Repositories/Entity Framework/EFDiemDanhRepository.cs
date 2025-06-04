using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using training_management_internship.Repositories.Interfaces;

namespace training_management_internship.Repositories
{
    public class EFDiemDanhRepository : IDiemDanhRepository
    {
        private readonly ApplicationDbContext _context;

        public EFDiemDanhRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DiemDanh>> GetAllAsync()
        {
            return await _context.DiemDanhs
                .Include(dd => dd.ChiTietLop)
                .Include(dd => dd.HocVien)
                .ToListAsync();
        }

        public async Task<DiemDanh?> GetByIdAsync(int id)
        {
            return await _context.DiemDanhs
                .Include(dd => dd.ChiTietLop)
                .Include(dd => dd.HocVien)
                .FirstOrDefaultAsync(dd => dd.DiemDanhId == id);
        }

        public async Task<IEnumerable<DiemDanh>> GetByChiTietLopIdAsync(int chiTietLopId)
        {
            return await _context.DiemDanhs
                .Where(dd => dd.ChiTietLopId == chiTietLopId)
                .Include(dd => dd.HocVien)
                .ToListAsync();
        }

        public async Task<IEnumerable<DiemDanh>> GetByHocVienIdAsync(int hocVienId)
        {
            return await _context.DiemDanhs
                .Where(dd => dd.HocVienId == hocVienId)
                .Include(dd => dd.ChiTietLop)
                .ToListAsync();
        }

        public async Task AddAsync(DiemDanh diemDanh)
        {
            _context.DiemDanhs.Add(diemDanh);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DiemDanh diemDanh)
        {
            _context.DiemDanhs.Update(diemDanh);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var diemDanh = await _context.DiemDanhs.FindAsync(id);
            if (diemDanh != null)
            {
                _context.DiemDanhs.Remove(diemDanh);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DiemDanhs.AnyAsync(dd => dd.DiemDanhId == id);
        }
    }
}
