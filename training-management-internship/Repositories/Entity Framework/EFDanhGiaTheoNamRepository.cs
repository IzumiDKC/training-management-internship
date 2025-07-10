using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using training_management_internship.Repositories.Interfaces.training_management_internship.Repositories.Interfaces;

namespace training_management_internship.Repositories.Entity_Framework
{
    public class EFDanhGiaTheoNamRepository : IDanhGiaTheoNamRepository
    {
        private readonly ApplicationDbContext _context;

        public EFDanhGiaTheoNamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DanhGiaTheoNam>> GetAllAsync()
        {
            return await _context.DanhGiaTheoNams
                .Include(d => d.HocVien).ThenInclude(h => h.User)
                .Include(d => d.NguoiDanhGia)
                .ToListAsync();
        }

        public async Task<DanhGiaTheoNam?> GetByIdAsync(int id)
        {
            return await _context.DanhGiaTheoNams
                .Include(d => d.HocVien).ThenInclude(h => h.User)
                .Include(d => d.NguoiDanhGia)
                .FirstOrDefaultAsync(d => d.DanhGiaTheoNamId == id);
        }

        public async Task AddAsync(DanhGiaTheoNam danhGia)
        {
            _context.DanhGiaTheoNams.Add(danhGia);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DanhGiaTheoNam danhGia)
        {
            _context.DanhGiaTheoNams.Update(danhGia);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.DanhGiaTheoNams.FindAsync(id);
            if (item != null)
            {
                _context.DanhGiaTheoNams.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DanhGiaTheoNams.AnyAsync(d => d.DanhGiaTheoNamId == id);
        }
    }
}
