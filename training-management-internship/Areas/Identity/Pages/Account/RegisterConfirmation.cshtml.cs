// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Text;
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
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _sender;

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            returnUrl ??= Url.Content("~/");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);

            await _sender.SendEmailAsync(email, "Xác nhận đăng ký tài khoản - Hệ thống đào tạo",
                $@"
                    <p>Xin chào,</p>

                    <p>Cảm ơn bạn đã đăng ký tài khoản tại <strong>Hệ thống Quản lý Đào tạo</strong>.</p>

                    <p>Để hoàn tất quá trình đăng ký, vui lòng xác nhận địa chỉ email của bạn bằng cách nhấn vào liên kết dưới đây:</p>

                    <p><a href='{callbackUrl}' target='_blank' style='color: #1a73e8;'>Xác nhận tài khoản</a></p>

                    <p>Nếu bạn không thực hiện hành động này, vui lòng bỏ qua email này.</p>

                    <br/>
                    <p>Trân trọng,</p>
                    <p><strong>Ban quản trị Hệ thống Đào tạo</strong></p>
                    
                ");

            // KHÔNG hiển thị link xác nhận trên giao diện nữa
            DisplayConfirmAccountLink = false;

            return Page();
        }

    }
}
