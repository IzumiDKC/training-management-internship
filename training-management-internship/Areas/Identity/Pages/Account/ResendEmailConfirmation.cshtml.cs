using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using training_management_internship.Models;

namespace training_management_internship.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ResendEmailConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản với email này. Vui lòng kiểm tra lại.");
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);

            var emailBody = $@"
                <p>Xin chào,</p>

                <p>Chúng tôi nhận thấy bạn chưa xác nhận email của mình. Để hoàn tất việc đăng ký tài khoản, vui lòng xác nhận email của bạn bằng cách nhấn vào liên kết dưới đây:</p>

                <p><a href='{HtmlEncoder.Default.Encode(callbackUrl)}' target='_blank' style='color: #1a73e8;'>Xác nhận tài khoản</a></p>

                <p>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>

                <br/>
                <p>Trân trọng,</p>
                <p><strong>Ban quản trị Hệ thống Đào tạo</strong></p>
            ";

            await _emailSender.SendEmailAsync(
                Input.Email,
                "Xác nhận email của bạn - Hệ thống Quản lý Đào tạo",
                emailBody);

            ModelState.AddModelError(string.Empty, "Email xác nhận đã được gửi. Vui lòng kiểm tra hộp thư của bạn.");
            return Page();
        }
    }
}
