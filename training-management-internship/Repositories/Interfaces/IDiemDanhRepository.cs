using training_management_internship.Models;

namespace training_management_internship.Repositories.Interfaces
{
    public interface IDiemDanhRepository
    {
        Task<IEnumerable<DiemDanh>> GetAllAsync();
        Task<DiemDanh?> GetByIdAsync(int id);
        Task<IEnumerable<DiemDanh>> GetByChiTietLopIdAsync(int chiTietLopId);
        Task<IEnumerable<DiemDanh>> GetByHocVienIdAsync(int hocVienId);
        Task AddAsync(DiemDanh diemDanh);
        Task UpdateAsync(DiemDanh diemDanh);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
