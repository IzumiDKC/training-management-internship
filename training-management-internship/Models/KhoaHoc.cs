using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace training_management_internship.Models
{
    public class KhoaHoc
    {
        public int KhoaHocId { get; set; }
        [Required]
        public string TenKhoaHoc { get; set; }

        public int ChuongTrinhDaoTaoId { get; set; }
        [ValidateNever]

        public virtual ChuongTrinhDaoTao ChuongTrinhDaoTao { get; set; }

        public virtual ICollection<Lop> Lops { get; set; }
        public virtual ICollection<DangKyKhoaHoc> DangKyKhoaHocs { get; set; } // 1->n

        public KhoaHoc()
        {
            Lops = new List<Lop>();
            DangKyKhoaHocs = new List<DangKyKhoaHoc>();
        }
    }

}
