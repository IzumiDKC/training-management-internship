namespace training_management_internship.Models
{
    public class DanhGiaTheoNam
    {
        public int DanhGiaTheoNamId { get; set; }

        public int HocVienId { get; set; }
        public virtual HocVien HocVien { get; set; }

        public int Nam { get; set; }

        public string LoaiDanhGia { get; set; }
        public string NoiDung { get; set; }

        public DateTime NgayDanhGia { get; set; } = DateTime.Now;

        public string NguoiDanhGiaId { get; set; }
        public virtual ApplicationUser NguoiDanhGia { get; set; }
    }


}
