using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace training_management_internship.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? HoTen { get; set; }

        // Navigation properties - excep: Admin
        public HocVien? HocVien { get; set; }
        public GiangVien? GiangVien { get; set; }
    }
}
