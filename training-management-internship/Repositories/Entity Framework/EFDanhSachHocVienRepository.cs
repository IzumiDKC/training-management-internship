using Microsoft.EntityFrameworkCore;
using training_management_internship.Models;
using training_management_internship.Repositories.Interfaces;

namespace training_management_internship.Repositories
{
    public class EFDanhSachHocVienRepository : IDanhSachHocVienRepository
    {
        private readonly ApplicationDbContext _context;

        public EFDanhSachHocVienRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DanhSachHocVien>> GetAllAsync()
        {
            return await _context.DanhSachHocViens
                .Include(ds => ds.Lop)
                .Include(ds => ds.HocVien)
                .ToListAsync();
        }

        public async Task<DanhSachHocVien?> GetByIdAsync(int id)
        {
            return await _context.DanhSachHocViens
                .Include(ds => ds.Lop)
                .Include(ds => ds.HocVien)
                .FirstOrDefaultAsync(ds => ds.DanhSachHocVienId == id);
        }

        public async Task<IEnumerable<DanhSachHocVien>> GetByLopIdAsync(int lopId)
        {
            return await _context.DanhSachHocViens
                .Where(ds => ds.LopId == lopId)
                .Include(ds => ds.HocVien)
                .ToListAsync();
        }

        public async Task<IEnumerable<DanhSachHocVien>> GetByHocVienIdAsync(int hocVienId)
        {
            return await _context.DanhSachHocViens
                .Where(ds => ds.HocVienId == hocVienId)
                .Include(ds => ds.Lop)
                .ToListAsync();
        }

        public async Task AddAsync(DanhSachHocVien danhSach)
        {
            _context.DanhSachHocViens.Add(danhSach);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DanhSachHocVien danhSach)
        {
            _context.DanhSachHocViens.Update(danhSach);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var danhSach = await _context.DanhSachHocViens.FindAsync(id);
            if (danhSach != null)
            {
                _context.DanhSachHocViens.Remove(danhSach);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DanhSachHocViens.AnyAsync(ds => ds.DanhSachHocVienId == id);
        }
    }
}
