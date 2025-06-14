using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace training_management_internship.Models
{
    public class ChiTietLop
    {
        public int ChiTietLopId { get; set; }
        public DateTime NgayHoc { get; set; }
        public TimeSpan ThoiGianBatDau { get; set; }
        public TimeSpan ThoiGianKetThuc { get; set; }

        public int LopId { get; set; }

        [ValidateNever]
        public virtual Lop Lop { get; set; }

        public int? GiangVienId { get; set; }

        [ValidateNever]
        public GiangVien? GiangVien { get; set; }

        [ValidateNever]
        public virtual ICollection<DiemDanh> DiemDanhs { get; set; }
    }

}
