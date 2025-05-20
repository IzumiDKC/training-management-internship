using training_management_internship.Models;

namespace training_management_internship.Repositories.Interfaces
{
    public interface IDangKyRepository
    {
        Task<IEnumerable<DangKyKhoaHoc>> GetAllAsync();
        Task<DangKyKhoaHoc?> GetByIdAsync(int id);
        Task<IEnumerable<DangKyKhoaHoc>> GetByHocVienIdAsync(int hocVienId);
        Task<IEnumerable<DangKyKhoaHoc>> GetByKhoaHocIdAsync(int khoaHocId);
        Task AddAsync(DangKyKhoaHoc dangKy);
        Task UpdateAsync(DangKyKhoaHoc dangKy);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
