namespace training_management_internship.Models
{
    public class DangKyKhoaHoc
    {
        public int DangKyKhoaHocId { get; set; }

        // Foreign Keys
        public int HocVienId { get; set; }
        public int KhoaHocId { get; set; }

        public DateTime NgayDangKy { get; set; }

        // Navigation
        public virtual HocVien HocVien { get; set; }
        public virtual KhoaHoc KhoaHoc { get; set; }

        public virtual ICollection<DanhGia> DanhGias { get; set; }
    }

}
