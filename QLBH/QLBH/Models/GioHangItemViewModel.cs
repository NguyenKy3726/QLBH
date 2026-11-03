namespace QLBH.Models
{
    public class GioHangItemViewModel
    {
        public string MaSP { get; set; } = string.Empty;
        public string TenSP { get; set; } = string.Empty;
        public string? AnhSP { get; set; }
        public decimal DonGia { get; set; }
        public short SoLuong { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
