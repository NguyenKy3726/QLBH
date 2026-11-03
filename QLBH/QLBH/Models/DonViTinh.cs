using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    public class DonViTinh
    {
        [Key]
        public byte MaDVT { get; set; }
        public string TenDVT { get; set; }
    }
}
