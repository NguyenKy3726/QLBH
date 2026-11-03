using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    public class SanPham
    {
        [Key]
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public decimal DonGia { get; set; }
        public string? AnhSP { get; set; }
        public string? MoTaCT { get; set; }
        public short MaLSP { get; set; }
        public byte MaDVT { get; set; }
    }
}
