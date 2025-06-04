using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using training_management_internship.Repositories.Interfaces;

namespace training_management_internship.Repositories
{
    public class EFChiTietLopRepository : IChiTietLopRepository
    {
        private readonly ApplicationDbContext _context;

        public EFChiTietLopRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChiTietLop>> GetAllAsync()
        {
            return await _context.ChiTietLops
                .Include(ct => ct.Lop)
                .Include(ct => ct.GiangVien)
                .ToListAsync();
        }

        public async Task<ChiTietLop?> GetByIdAsync(int id)
        {
            return await _context.ChiTietLops
                .Include(ct => ct.Lop)
                .Include(ct => ct.GiangVien)
                .FirstOrDefaultAsync(ct => ct.ChiTietLopId == id);
        }

        public async Task AddAsync(ChiTietLop chiTietLop)
        {
            _context.ChiTietLops.Add(chiTietLop);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChiTietLop chiTietLop)
        {
            _context.ChiTietLops.Update(chiTietLop);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var chiTietLop = await _context.ChiTietLops.FindAsync(id);
            if (chiTietLop != null)
            {
                _context.ChiTietLops.Remove(chiTietLop);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ChiTietLops.AnyAsync(ct => ct.ChiTietLopId == id);
        }
    }
}
