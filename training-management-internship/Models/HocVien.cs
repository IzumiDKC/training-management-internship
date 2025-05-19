namespace training_management_internship.Models
{
    public class HocVien
    {
        public int HocVienId { get; set; }
        public DateTime NgaySinh { get; set; }
        public int TongKhoaHoc { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public virtual ICollection<DangKyKhoaHoc> DangKyKhoaHocs { get; set; }
    }
}
