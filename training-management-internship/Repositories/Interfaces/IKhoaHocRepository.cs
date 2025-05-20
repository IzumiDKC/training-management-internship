using training_management_internship.Models;

namespace training_management_internship.Repositories.Interfaces
{
    public interface IKhoaHocRepository
    {
            Task<IEnumerable<KhoaHoc>> GetAllAsync();
            Task<KhoaHoc?> GetByIdAsync(int id);
            Task AddAsync(KhoaHoc khoaHoc);
            Task UpdateAsync(KhoaHoc khoaHoc);
            Task DeleteAsync(int id);
        
    }
}
