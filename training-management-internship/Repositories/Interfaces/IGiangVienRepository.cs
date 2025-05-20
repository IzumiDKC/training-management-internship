using training_management_internship.Models;

namespace training_management_internship.Repositories.Interfaces
{
    public interface IGiangVienRepository
    {
        Task<IEnumerable<GiangVien>> GetAllAsync();
        Task<GiangVien?> GetByIdAsync(int id);
        Task AddAsync(GiangVien giangVien);
        Task UpdateAsync(GiangVien giangVien);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
