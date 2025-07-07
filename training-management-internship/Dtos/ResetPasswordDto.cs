using System.ComponentModel.DataAnnotations;

public class ResetPasswordDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }

    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}
