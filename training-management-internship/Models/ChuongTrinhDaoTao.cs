namespace training_management_internship.Models
{
    public class ChuongTrinhDaoTao
    {
        public int ChuongTrinhDaoTaoId { get; set; }
        public string TenChuongTrinh { get; set; }
        public string MoTa { get; set; }

        // Navigation
        public virtual ICollection<KhoaHoc> KhoaHocs { get; set; }
    }

}
