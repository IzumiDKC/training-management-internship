using training_management_internship.Models;

public class DanhGia
{
    public int DanhGiaId { get; set; }

    public int HocVienId { get; set; }
    public virtual HocVien HocVien { get; set; }

    public int LopId { get; set; }
    public virtual Lop Lop { get; set; }

    public string LoaiDanhGia { get; set; }
    public string NoiDung { get; set; }

    public DateTime NgayDanhGia { get; set; } = DateTime.Now;

    public string NguoiDanhGiaId { get; set; }
    public virtual ApplicationUser NguoiDanhGia { get; set; }
}
