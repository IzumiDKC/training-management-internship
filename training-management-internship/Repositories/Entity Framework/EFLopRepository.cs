using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using training_management_internship.Repositories.Interfaces;

namespace training_management_internship.Repositories
{
    public class EFLopRepository : ILopRepository
    {
        private readonly ApplicationDbContext _context;

        public EFLopRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lop>> GetAllAsync()
        {
            return await _context.Lops
                .Include(l => l.KhoaHoc)
                .Include(l => l.LoaiLop)
                .Include(l => l.ChiTietLops)
                .Include(l => l.DanhSachHocViens)
                .ToListAsync();
        }

        public async Task<Lop?> GetByIdAsync(int id)
        {
            return await _context.Lops
                .Include(l => l.KhoaHoc)
                .Include(l => l.LoaiLop)
                .Include(l => l.ChiTietLops)
                .Include(l => l.DanhSachHocViens)
                .FirstOrDefaultAsync(l => l.LopId == id);
        }

        public async Task<IEnumerable<Lop>> GetByKhoaHocIdAsync(int khoaHocId)
        {
            return await _context.Lops
                .Where(l => l.KhoaHocId == khoaHocId)
                .Include(l => l.LoaiLop)
                .Include(l => l.ChiTietLops)
                .ToListAsync();
        }

        public async Task AddAsync(Lop lop)
        {
            _context.Lops.Add(lop);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Lop lop)
        {
            _context.Lops.Update(lop);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var lop = await _context.Lops.FindAsync(id);
            if (lop != null)
            {
                _context.Lops.Remove(lop);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Lops.AnyAsync(l => l.LopId == id);
        }
    }
}
