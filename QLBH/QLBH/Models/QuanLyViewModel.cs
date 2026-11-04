namespace QLBH.Models;

public class QuanLyViewModel
{
    public string TenNhanVien { get; init; } = "Nhân viên";
    public List<QuanLyNhomViewModel> NhomChucNang { get; init; } = new();
}

public class QuanLyNhomViewModel
{
    public string TieuDe { get; init; } = string.Empty;
    public string MoTa { get; init; } = string.Empty;
    public string BieuTuong { get; init; } = string.Empty;
    public List<QuanLyChucNangViewModel> ChucNang { get; init; } = new();
}

public class QuanLyChucNangViewModel
{
    public string TieuDe { get; init; } = string.Empty;
    public string MoTa { get; init; } = string.Empty;
    public string Controller { get; init; } = string.Empty;
}
