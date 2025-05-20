using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using training_management_internship.Repositories.Interfaces;

namespace training_management_internship.Repositories
{
    public class EFChuongTrinhRepository : IChuongTrinhRepository
    {
        private readonly ApplicationDbContext _context;

        public EFChuongTrinhRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChuongTrinhDaoTao>> GetAllAsync()
        {
            return await _context.ChuongTrinhDaoTaos
                .Include(ct => ct.KhoaHocs)
                .ToListAsync();
        }

        public async Task<ChuongTrinhDaoTao?> GetByIdAsync(int id)
        {
            return await _context.ChuongTrinhDaoTaos
                .Include(ct => ct.KhoaHocs)
                .FirstOrDefaultAsync(ct => ct.ChuongTrinhDaoTaoId == id);
        }

        public async Task AddAsync(ChuongTrinhDaoTao chuongTrinh)
        {
            _context.ChuongTrinhDaoTaos.Add(chuongTrinh);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChuongTrinhDaoTao chuongTrinh)
        {
            _context.ChuongTrinhDaoTaos.Update(chuongTrinh);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var chuongTrinh = await _context.ChuongTrinhDaoTaos.FindAsync(id);
            if (chuongTrinh != null)
            {
                _context.ChuongTrinhDaoTaos.Remove(chuongTrinh);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ChuongTrinhDaoTaos.AnyAsync(ct => ct.ChuongTrinhDaoTaoId == id);
        }
    }
}
