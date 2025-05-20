using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using training_management_internship.Repositories.Interfaces;

namespace training_management_internship.Repositories
{
    public class EFGiangVienRepository : IGiangVienRepository
    {
        private readonly ApplicationDbContext _context;

        public EFGiangVienRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GiangVien>> GetAllAsync()
        {
            return await _context.GiangViens
                .Include(gv => gv.KhoaHocs)
                .ToListAsync();
        }

        public async Task<GiangVien?> GetByIdAsync(int id)
        {
            return await _context.GiangViens
                .Include(gv => gv.KhoaHocs)
                .FirstOrDefaultAsync(gv => gv.GiangVienId == id);
        }

        public async Task AddAsync(GiangVien giangVien)
        {
            _context.GiangViens.Add(giangVien);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GiangVien giangVien)
        {
            _context.GiangViens.Update(giangVien);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var gv = await _context.GiangViens.FindAsync(id);
            if (gv != null)
            {
                _context.GiangViens.Remove(gv);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.GiangViens.AnyAsync(gv => gv.GiangVienId == id);
        }
    }
}
