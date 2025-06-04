using training_management_internship.Models;

namespace training_management_internship.Repositories.Interfaces
{
    public interface IDanhSachHocVienRepository
    {
        Task<IEnumerable<DanhSachHocVien>> GetAllAsync();
        Task<DanhSachHocVien?> GetByIdAsync(int id);
        Task<IEnumerable<DanhSachHocVien>> GetByLopIdAsync(int lopId);
        Task<IEnumerable<DanhSachHocVien>> GetByHocVienIdAsync(int hocVienId);
        Task AddAsync(DanhSachHocVien danhSach);
        Task UpdateAsync(DanhSachHocVien danhSach);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
