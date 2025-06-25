namespace training_management_internship.Models
{
    public class ChiTietLopDto
    {
        public int ChiTietLopId { get; set; }
        public DateTime NgayHoc { get; set; }
        public TimeSpan ThoiGianBatDau { get; set; }
        public TimeSpan ThoiGianKetThuc { get; set; }

        public int LopId { get; set; }
        public string? TenLop { get; set; }

        public int? GiangVienId { get; set; }
        public string? TenGiangVien { get; set; }

        public string? GioHoc => $"{ThoiGianBatDau:hh\\:mm} - {ThoiGianKetThuc:hh\\:mm}";
    }
}
