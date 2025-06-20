using System.ComponentModel.DataAnnotations;

namespace training_management_internship.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Số căn cước phải gồm đúng 12 chữ số.")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Số căn cước phải là 12 chữ số.")]
        public string SoCanCuoc { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; } = "HocVien";
    }
}
