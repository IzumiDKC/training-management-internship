using training_management_internship.Models;

namespace training_management_internship.Repositories.Interfaces
{
    public interface IDanhGiaRepository
    {
        Task<IEnumerable<DanhGia>> GetAllAsync();
        Task<DanhGia?> GetByIdAsync(int id);
        Task AddAsync(DanhGia danhGia);
        Task UpdateAsync(DanhGia danhGia);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
