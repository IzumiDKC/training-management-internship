using training_management_internship.Models;

namespace training_management_internship.Repositories.Interfaces
{

    namespace training_management_internship.Repositories.Interfaces
    {
        public interface IDanhGiaTheoNamRepository
        {
            Task<IEnumerable<DanhGiaTheoNam>> GetAllAsync();
            Task<DanhGiaTheoNam?> GetByIdAsync(int id);
            Task AddAsync(DanhGiaTheoNam danhGia);
            Task UpdateAsync(DanhGiaTheoNam danhGia);
            Task DeleteAsync(int id);
            Task<bool> ExistsAsync(int id);
        }
    }

}
