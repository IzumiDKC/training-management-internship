namespace training_management_internship.Models
{
    public class KhoaHoc
    {
        public int KhoaHocId { get; set; }
        public string TenKhoaHoc { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }

        // Foreign Keys
        public int GiangVienId { get; set; }
        public int ChuongTrinhDaoTaoId { get; set; }

        // Navigation
        public virtual GiangVien GiangVien { get; set; }
        public virtual ChuongTrinhDaoTao ChuongTrinhDaoTao { get; set; }
        public virtual ICollection<DangKyKhoaHoc> DangKyKhoaHocs { get; set; }
    }

}
