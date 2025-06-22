using System;
using System.Collections.Generic;

namespace training_management_internship.Models
{
    public class LopDto
    {
        public int LopId { get; set; }
        public string TenLop { get; set; }
        public DateTime NgayBatDauDuKien { get; set; }
        public DateTime NgayKetThucDuKien { get; set; }
        public int SoGio { get; set; }
        public int SoGioQuyDoi { get; set; }
        public bool CoDanhSachHocVien { get; set; }
        public int KhoaHocId { get; set; }
        public string KhoaHocName { get; set; }  
        public int LoaiLopId { get; set; }
        public string LoaiLopName { get; set; } 
        public ICollection<int> DanhSachHocVienIds { get; set; } 
    }
}
