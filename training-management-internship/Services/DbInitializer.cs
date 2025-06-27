using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using training_management_internship.Models;
using Microsoft.EntityFrameworkCore;

namespace training_management_internship.Services
{
    public static class DbInitializer
    {
        public static async Task SeedAccountsAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Tạo role nếu chưa có
            var roles = new[] { "Admin", "GiangVien", "HocVien" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            await CreateUserIfNotExists(userManager, context, "huudien111@gmail.com", "Administrator", "000000000000", "Admin");

            await CreateUserIfNotExists(userManager, context, "giangvien@example.com", "Giang Vien Account", "111111111111", "GiangVien");

            await CreateUserIfNotExists(userManager, context, "hocvien@example.com", "Hoc Vien Account", "222222222222", "HocVien");
        }

        private static async Task CreateUserIfNotExists(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            string email,
            string hoTen,
            string soCanCuoc,
            string role)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    HoTen = hoTen,
                    SoCanCuoc = soCanCuoc,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, $"{role}@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
            else
            {
                if (!user.EmailConfirmed)
                {
                    user.EmailConfirmed = true;
                    await userManager.UpdateAsync(user);
                }

                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }

            // Gán role -> Table GiangVien / HocVien
            if (role == "GiangVien")
            {
                var exists = await context.GiangViens.AnyAsync(g => g.UserId == user.Id);
                if (!exists)
                {
                    context.GiangViens.Add(new GiangVien { UserId = user.Id });
                    await context.SaveChangesAsync();
                }
            }
            else if (role == "HocVien")
            {
                var exists = await context.HocViens.AnyAsync(h => h.UserId == user.Id);
                if (!exists)
                {
                    context.HocViens.Add(new HocVien { UserId = user.Id });
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
