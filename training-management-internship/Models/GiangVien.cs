namespace training_management_internship.Models
{
    public class GiangVien
    {
        public int GiangVienId { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        // Navigation
        public virtual ICollection<KhoaHoc> KhoaHocs { get; set; }
    }

}
