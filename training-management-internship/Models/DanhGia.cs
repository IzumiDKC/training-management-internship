using System.ComponentModel.DataAnnotations;

namespace training_management_internship.Models
{
    public class DanhGia
    {
        public int DanhGiaId { get; set; }
        public int DangKyKhoaHocId { get; set; }

        public string LoaiDanhGia { get; set; }
        public string NoiDung { get; set; }

        public virtual DangKyKhoaHoc DangKyKhoaHoc { get; set; }
        public DateTime NgayDanhGia { get; set; }

    }

}
