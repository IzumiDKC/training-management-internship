using training_management_internship.Models;

namespace training_management_internship.Repositories.Interfaces
{
    public interface IChiTietLopRepository
    {
        Task<IEnumerable<ChiTietLop>> GetAllAsync();
        Task<ChiTietLop?> GetByIdAsync(int id);
        Task AddAsync(ChiTietLop chiTietLop);
        Task UpdateAsync(ChiTietLop chiTietLop);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
