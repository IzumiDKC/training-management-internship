namespace training_management_internship.Dtos
{
    public class KhoaHocDetailDto
    {
        public int KhoaHocId { get; set; }
        public string TenKhoaHoc { get; set; }

        public ChuongTrinhShortDto ChuongTrinhDaoTao { get; set; }
    }

    public class ChuongTrinhShortDto
    {
        public int ChuongTrinhDaoTaoId { get; set; }
        public string TenChuongTrinh { get; set; }
    }
}
