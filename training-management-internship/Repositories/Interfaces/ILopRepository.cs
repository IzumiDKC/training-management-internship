using training_management_internship.Models;

namespace training_management_internship.Repositories.Interfaces
{
    public interface ILopRepository
    {
        Task<IEnumerable<Lop>> GetAllAsync();
        Task<Lop?> GetByIdAsync(int id);
        Task<IEnumerable<Lop>> GetByKhoaHocIdAsync(int khoaHocId);
        Task AddAsync(Lop lop);
        Task UpdateAsync(Lop lop);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
