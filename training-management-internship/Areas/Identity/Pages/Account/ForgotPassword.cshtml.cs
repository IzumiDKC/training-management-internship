using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using training_management_internship.Models;

namespace training_management_internship.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ForgotPasswordModel> _logger;
        private readonly IConfiguration _configuration;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, ILogger<ForgotPasswordModel> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user == null)
                    {
                        _logger.LogWarning("Email không tồn tại: {Email}", Input.Email);
                        ModelState.AddModelError(string.Empty, "Email không tồn tại. Vui lòng kiểm tra lại.");
                        return Page();
                    }
                    else if (!(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        _logger.LogWarning("Email chưa được xác nhận: {Email}", Input.Email);
                        ModelState.AddModelError(string.Empty, "Email chưa được xác nhận. Vui lòng kiểm tra email của bạn.");
                        return Page();
                    }

                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var frontendUrl = _configuration["Frontend:BaseUrl"];
                    var callbackUrl = $"{frontendUrl}/reset-password?code={code}&email={Input.Email}";

                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "Yêu cầu thay đổi mật khẩu",
                        $@"
                            <p>Chào bạn,</p>

                            <p>Chúng tôi đã nhận được yêu cầu thay đổi mật khẩu từ bạn. Để thay đổi mật khẩu, vui lòng nhấn vào liên kết dưới đây:</p>

                            <p><a href='{HtmlEncoder.Default.Encode(callbackUrl)}' target='_blank' style='color: #1a73e8;'>Thay đổi mật khẩu</a></p>

                            <p>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>

                            <br/>
                            <p>Trân trọng,</p>
                            <p><strong>Ban quản trị Hệ thống Quản lý Đào tạo</strong></p>
                        ");

                    _logger.LogInformation("Email yêu cầu thay đổi mật khẩu đã được gửi đến {Email}", Input.Email);
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Đã xảy ra lỗi khi gửi email thay đổi mật khẩu tới {Email}", Input.Email);
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi gửi email. Vui lòng thử lại sau.");
                }
            }

            return Page();
        }
    }
}
