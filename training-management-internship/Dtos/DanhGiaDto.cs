namespace training_management_internship.Dtos
{
    public class DanhGiaCreateDto
    {
        public int HocVienId { get; set; }
        public int LopId { get; set; }
        public string LoaiDanhGia { get; set; }
        public string NoiDung { get; set; }
    }

    public class DanhGiaDto
    {
        public int DanhGiaId { get; set; }
        public string HoTen { get; set; }
        public string SoCanCuoc { get; set; }
        public string TenLop { get; set; }
        public string LoaiDanhGia { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayDanhGia { get; set; }
        public string NguoiDanhGia { get; set; }
    }

    public class DanhGiaChiTietDto
    {
        public string LoaiDanhGia { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayDanhGia { get; set; }
    }


}
