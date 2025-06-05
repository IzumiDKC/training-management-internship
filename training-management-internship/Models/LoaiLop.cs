using System.ComponentModel.DataAnnotations;

namespace training_management_internship.Models
{
    public class LoaiLop
    {
        public int LoaiLopId { get; set; }

        [Required]
        public string TenLoaiLop { get; set; }

        public virtual ICollection<Lop> Lops { get; set; } // 1 Loai lop -> nhieu Lop

        public LoaiLop()
        {
            Lops = new List<Lop>();
        }

    }

}
