namespace training_management_internship.Dtos
{
    public class ChuongTrinhDto
    {
        public int ChuongTrinhDaoTaoId { get; set; }
        public string TenChuongTrinh { get; set; }
        public string MoTa { get; set; }

        public List<KhoaHocDto> KhoaHocs { get; set; }
    }
}
