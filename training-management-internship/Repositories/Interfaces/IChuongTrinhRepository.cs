using training_management_internship.Models;

namespace training_management_internship.Repositories.Interfaces
{
    public interface IChuongTrinhRepository
    {
        Task<IEnumerable<ChuongTrinhDaoTao>> GetAllAsync();
        Task<ChuongTrinhDaoTao?> GetByIdAsync(int id);
        Task AddAsync(ChuongTrinhDaoTao chuongTrinh);
        Task UpdateAsync(ChuongTrinhDaoTao chuongTrinh);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
