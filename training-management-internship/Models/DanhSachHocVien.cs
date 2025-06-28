namespace training_management_internship.Models
{
    public class DanhSachHocVien
    {
        public int DanhSachHocVienId { get; set; }

        public int LopId { get; set; }
        public virtual Lop Lop { get; set; }

        public int HocVienId { get; set; }
        public virtual HocVien HocVien { get; set; }
    }

}
    