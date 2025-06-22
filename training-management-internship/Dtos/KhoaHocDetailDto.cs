using training_management_internship.Models;

namespace training_management_internship.Dtos
{
    public class KhoaHocDetailDto
    {
        public int KhoaHocId { get; set; }
        public string TenKhoaHoc { get; set; }

        public ChuongTrinhShortDto ChuongTrinhDaoTao { get; set; }

        public List<LopDto> Lops { get; set; } 

    }

    public class ChuongTrinhShortDto
    {
        public int ChuongTrinhDaoTaoId { get; set; }
        public string TenChuongTrinh { get; set; }
    }
}
