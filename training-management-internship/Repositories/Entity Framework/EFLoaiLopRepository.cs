using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using training_management_internship.Repositories.Interfaces;

namespace training_management_internship.Repositories
{
    public class EFLoaiLopRepository : ILoaiLopRepository
    {
        private readonly ApplicationDbContext _context;

        public EFLoaiLopRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LoaiLop>> GetAllAsync()
        {
            return await _context.LoaiLops
                .Include(ll => ll.Lops) 
                .ToListAsync();
        }

        public async Task<LoaiLop?> GetByIdAsync(int id)
        {
            return await _context.LoaiLops
                .Include(ll => ll.Lops)
                .FirstOrDefaultAsync(ll => ll.LoaiLopId == id);
        }

        public async Task AddAsync(LoaiLop loaiLop)
        {
            _context.LoaiLops.Add(loaiLop);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LoaiLop loaiLop)
        {
            _context.LoaiLops.Update(loaiLop);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var loaiLop = await _context.LoaiLops.FindAsync(id);
            if (loaiLop != null)
            {
                _context.LoaiLops.Remove(loaiLop);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.LoaiLops.AnyAsync(ll => ll.LoaiLopId == id);
        }
    }
}
