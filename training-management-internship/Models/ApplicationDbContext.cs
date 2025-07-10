using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using training_management_internship.Services;

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

        public DbSet<Lop> Lops { get; set; }
        public DbSet<LoaiLop> LoaiLops { get; set; }
        public DbSet<ChiTietLop> ChiTietLops { get; set; }
        public DbSet<DiemDanh> DiemDanhs { get; set; }
        public DbSet<DanhSachHocVien> DanhSachHocViens { get; set; }
        public DbSet<QRCodeTemp> QRCodeTemps { get; set; }
        public DbSet<DanhGiaTheoNam> DanhGiaTheoNams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DangKyKhoaHoc>()
                .HasOne(d => d.Lop)
                .WithMany()
                .HasForeignKey(d => d.LopId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChiTietLop>()
                .HasOne(c => c.Lop)
                .WithMany(l => l.ChiTietLops)
                .HasForeignKey(c => c.LopId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DanhSachHocVien>()
                .HasOne(ds => ds.Lop)
                .WithMany(l => l.DanhSachHocViens)
                .HasForeignKey(ds => ds.LopId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
