using training_management_internship.Models;

namespace training_management_internship.Repositories.Interfaces
{
    public interface ILoaiLopRepository
    {
        Task<IEnumerable<LoaiLop>> GetAllAsync();
        Task<LoaiLop?> GetByIdAsync(int id);
   //     Task<IEnumerable<LoaiLop>> GetByLopIdAsync(int lopId);
        Task AddAsync(LoaiLop loaiLop);
        Task UpdateAsync(LoaiLop loaiLop);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
