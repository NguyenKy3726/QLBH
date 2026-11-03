using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    public class NhaCC
    {
        [Key]
        public short MaNCC { get; set; }
        public string TenNCC { get; set; }
        public string? DienThoai { get; set; }
        public string? Email { get; set; }
        public string DiaChi { get; set; }
        public string MaXP { get; set; }
    }
}
