using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace training_management_internship.Models
{
    public class Lop
    {
        public int LopId { get; set; }
        [Required]
        public string TenLop { get; set; }
        public DateTime NgayBatDauDuKien { get; set; }
        public DateTime NgayKetThucDuKien { get; set; }
        public int SoGio { get; set; }
        public int SoGioQuyDoi { get; set; }
        public bool CoDanhSachHocVien { get; set; }
       // [ForeignKey("KhoaHoc")]
        public int KhoaHocId { get; set; }

        [ValidateNever]
        public virtual KhoaHoc KhoaHoc { get; set; }

        public int LoaiLopId { get; set; }

        [ValidateNever]
        public virtual LoaiLop LoaiLop { get; set; } // 1 lop -> 1 loai lop

        [ValidateNever]
        public virtual ICollection<ChiTietLop> ChiTietLops { get; set; }

        [ValidateNever]
        public virtual ICollection<DanhSachHocVien> DanhSachHocViens { get; set; }
    }

}   
