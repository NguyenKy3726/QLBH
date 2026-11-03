using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    public class NhanVien
    {
        [Key]
        public byte MaNV { get; set; }
        public string HoNV { get; set; }
        public string TenNV { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string TenDN { get; set; }
        public byte[] MatKhau { get; set; }
        public string? AnhNV { get; set; }
        public byte MaPB { get; set; }
    }
}
