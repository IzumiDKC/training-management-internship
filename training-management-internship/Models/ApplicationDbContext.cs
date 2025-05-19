using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace training_management_internship.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<HocVien> HocViens { get; set; }
        public DbSet<GiangVien> GiangViens { get; set; }
        public DbSet<ChuongTrinhDaoTao> ChuongTrinhDaoTaos { get; set; }
        public DbSet<KhoaHoc> KhoaHocs { get; set; }
        public DbSet<DangKyKhoaHoc> DangKyKhoaHocs { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }
    }
}
