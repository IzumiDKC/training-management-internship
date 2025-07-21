using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace training_management_internship.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? HoTen { get; set; }
        public string? NoiCongTac { get; set; }
        public DateTime NgaySinh { get; set; }

        [Required]
        public string SoCanCuoc { get; set; }
        public string? HocHamHocVi { get; set; }
        public bool ThuocBenhVien { get; set; }
        public HocVien? HocVien { get; set; }
        public GiangVien? GiangVien { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
