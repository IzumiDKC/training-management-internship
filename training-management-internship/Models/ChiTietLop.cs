namespace training_management_internship.Models
{
    public class ChiTietLop
    {
        public int ChiTietLopId { get; set; }
        public DateTime NgayHoc { get; set; }
        public TimeSpan ThoiGianBatDau { get; set; }
        public TimeSpan ThoiGianKetThuc { get; set; }

        public int LopId { get; set; }
        public virtual Lop Lop { get; set; }

        public int? GiangVienId { get; set; }
        public GiangVien? GiangVien { get; set; }
        public virtual ICollection<DiemDanh> DiemDanhs { get; set; }
    }

}
