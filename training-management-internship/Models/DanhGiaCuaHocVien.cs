namespace training_management_internship.Models
{
    public class DanhGiaCuaHocVien
    {
        public int DanhGiaCuaHocVienId { get; set; }
        public int HocVienId { get; set; }
        public int KhoaHocId { get; set; }

        public int Diem { get; set; } // điểm đánh giá 1-5
        public string NhanXet { get; set; }

        public virtual HocVien HocVien { get; set; }
        public virtual KhoaHoc KhoaHoc { get; set; }
    }

}
