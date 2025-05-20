using training_management_internship.Models;

namespace training_management_internship.Repositories.Interfaces
{
    public interface IHocVienRepository
    {
        Task<IEnumerable<HocVien>> GetAllAsync();
        Task<HocVien?> GetByIdAsync(int id);
        Task AddAsync(HocVien hocVien);
        Task UpdateAsync(HocVien hocVien);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
