using System.ComponentModel.DataAnnotations;

namespace training_management_internship.Models
{
    public class DanhGia
    {
        public int DanhGiaId { get; set; }
        public int DangKyKhoaHocId { get; set; }

        public string NoiDung { get; set; }

        [Required]
        public int Diem { get; set; } // 0-10

        public virtual DangKyKhoaHoc DangKyKhoaHoc { get; set; }
    }

}
