using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace training_management_internship.Models
{
    public class ChuongTrinhDaoTao
    {
        public int ChuongTrinhDaoTaoId { get; set; }
        public string TenChuongTrinh { get; set; }
        public string? MoTa { get; set; }

        public virtual ICollection<KhoaHoc> KhoaHocs { get; set; } = new List<KhoaHoc>();
    }
}s