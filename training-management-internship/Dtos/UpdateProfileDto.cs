namespace training_management_internship.Dtos
{
    public class UpdateProfileDto
    {
        public string HoTen { get; set; }
        public string? NoiCongTac { get; set; }
        public DateTime NgaySinh { get; set; }
        public string? HocHamHocVi { get; set; }
        public bool ThuocBenhVien { get; set; }
    }

}
