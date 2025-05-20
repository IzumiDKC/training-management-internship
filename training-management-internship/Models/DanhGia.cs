using System.ComponentModel.DataAnnotations;

namespace training_management_internship.Models
{
    public class DanhGia
    {
        public int DanhGiaId { get; set; }
        public int DangKyKhoaHocId { get; set; }

        public string NoiDung { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Điểm phải nằm trong khoảng từ 1 đến 10")]

        public int Diem { get; set; } // 0-10

        public virtual DangKyKhoaHoc DangKyKhoaHoc { get; set; }
        public DateTime NgayDanhGia { get; set; }

    }

}
