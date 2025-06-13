using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace training_management_internship.Models
{
    public class DangKyKhoaHoc
    {
        public int DangKyKhoaHocId { get; set; }

        // Foreign Keys
        public int HocVienId { get; set; }
        public int KhoaHocId { get; set; }

        public DateTime NgayDangKy { get; set; }

        [ValidateNever]
        public virtual HocVien HocVien { get; set; }

        [ValidateNever]
        public virtual KhoaHoc KhoaHoc { get; set; }

        [ValidateNever]
        public virtual ICollection<DanhGia> DanhGias { get; set; }

        public int? LopId { get; set; }

        [ValidateNever]
        public virtual Lop? Lop { get; set; }

    }

}
