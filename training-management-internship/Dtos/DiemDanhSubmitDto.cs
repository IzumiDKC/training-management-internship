namespace training_management_internship.Dtos
{
    public class DiemDanhSubmitDto
    {
        public int ChiTietLopId { get; set; }
        public int HocVienId { get; set; }
        public DateTime NgayCheck { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public string? Note { get; set; }
    }

}
