using System.ComponentModel.DataAnnotations;

namespace training_management_internship.Dtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
