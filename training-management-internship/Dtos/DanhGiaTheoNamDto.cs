namespace training_management_internship.Dtos
{
    public class DanhGiaTheoNamCreateDto
    {
        public int HocVienId { get; set; }
        public int Nam { get; set; }
        public string LoaiDanhGia { get; set; }
        public string NoiDung { get; set; }
    }

    public class DanhGiaTheoNamDto
    {
        public int Nam { get; set; }
        public string HoTen { get; set; }
        public string SoCanCuoc { get; set; }
        public string LoaiDanhGia { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayDanhGia { get; set; }
        public string NguoiDanhGia { get; set; }
    }

}
