// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

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
using training_management_internship.Models;

namespace training_management_internship.Areas.Identity.Pages.Account.Manage
{
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public EditModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Họ Tên")]
            public string? HoTen { get; set; }

            [Display(Name = "Nơi công tác")]
            public string? NoiCongTac { get; set; }

            [Display(Name = "Ngày sinh")]
            [DataType(DataType.Date)]
            public DateTime? NgaySinh { get; set; }

            [Display(Name = "Học hàm học vị")]
            public string? HocHamHocVi { get; set; }

            [Display(Name = "Thuộc bệnh viện")]
            public bool ThuocBenhVien { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("Không tìm thấy người dùng.");

            Input = new InputModel
            {
                HoTen = user.HoTen,
                NoiCongTac = user.NoiCongTac,
                NgaySinh = user.NgaySinh,
                HocHamHocVi = user.HocHamHocVi,
                ThuocBenhVien = user.ThuocBenhVien
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("Không tìm thấy người dùng.");

            if (!ModelState.IsValid) return Page();

            user.HoTen = Input.HoTen;
            user.NoiCongTac = Input.NoiCongTac;
            user.NgaySinh = Input.NgaySinh ?? user.NgaySinh;
            user.HocHamHocVi = Input.HocHamHocVi;
            user.ThuocBenhVien = Input.ThuocBenhVien;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["StatusMessage"] = "Thông tin đã được cập nhật.";
            return RedirectToPage(); // quay lại trang hiện tại
        }
    }

}
