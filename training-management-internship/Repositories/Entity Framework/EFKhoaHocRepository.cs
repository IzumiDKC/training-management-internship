using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using training_management_internship.Repositories.Interfaces;

namespace training_management_internship.Repositories.Entity_Framework
{
    public class EFKhoaHocRepository : IKhoaHocRepository
    {
        private readonly ApplicationDbContext _context;

        public EFKhoaHocRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KhoaHoc>> GetAllAsync()
        {
            return await _context.KhoaHocs.ToListAsync();
        }

        public async Task<KhoaHoc?> GetByIdAsync(int id)
        {
            return await _context.KhoaHocs.FindAsync(id);
        }

        public async Task AddAsync(KhoaHoc khoaHoc)
        {
            _context.KhoaHocs.Add(khoaHoc);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(KhoaHoc khoaHoc)
        {
            _context.KhoaHocs.Update(khoaHoc);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.KhoaHocs.FindAsync(id);
            if (entity != null)
            {
                _context.KhoaHocs.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

}
