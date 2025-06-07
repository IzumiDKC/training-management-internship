using System.Collections.Generic;

namespace training_management_internship.Models
{
    public class AdminDashboard
    {
        public IEnumerable<ChuongTrinhDaoTao> ChuongTrinhDaoTaos { get; set; }
        public IEnumerable<KhoaHoc> KhoaHocs { get; set; }
        public IEnumerable<Lop> Lops { get; set; }
        public IEnumerable<LoaiLop> LoaiLops { get; set; }
        public IEnumerable<GiangVien> GiangViens { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}