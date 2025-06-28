namespace training_management_internship.Models
{
    public class DiemDanhDto
    {
        public int DiemDanhId { get; set; }
        public DateTime NgayCheck { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public int HocVienId { get; set; }
        public string? Note { get; set; }

        public int ChiTietLopId { get; set; }

        public string HocVienName { get; set; }
        public string SoCanCuoc { get; set; }
    }
}
