using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using training_management_internship.Repositories.Interfaces;

namespace training_management_internship.Repositories
{
    public class EFHocVienRepository : IHocVienRepository
    {
        private readonly ApplicationDbContext _context;

        public EFHocVienRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HocVien>> GetAllAsync()
        {
            return await _context.HocViens
                .Include(hv => hv.DangKyKhoaHocs)
                .ToListAsync();
        }

        public async Task<HocVien?> GetByIdAsync(int id)
        {
            return await _context.HocViens
                .Include(hv => hv.DangKyKhoaHocs)
                .FirstOrDefaultAsync(hv => hv.HocVienId == id);
        }

        public async Task AddAsync(HocVien hocVien)
        {
            _context.HocViens.Add(hocVien);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HocVien hocVien)
        {
            _context.HocViens.Update(hocVien);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hv = await _context.HocViens.FindAsync(id);
            if (hv != null)
            {
                _context.HocViens.Remove(hv);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.HocViens.AnyAsync(hv => hv.HocVienId == id);
        }
    }
}
