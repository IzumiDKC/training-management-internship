namespace training_management_internship.Models
{
    public class DiemDanh
    {
        public int DiemDanhId { get; set; }
        public DateTime NgayCheck { get; set; }
        public TimeSpan CheckIn { get; set; }
        public TimeSpan CheckOut { get; set; }

        public int ChiTietLopId { get; set; }
        public virtual ChiTietLop ChiTietLop { get; set; }

        public int HocVienId { get; set; }
        public virtual HocVien HocVien { get; set; }
    }

}
