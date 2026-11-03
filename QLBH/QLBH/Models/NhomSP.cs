using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    public class NhomSP
    {
        [Key]
        public byte MaNSP { get; set; }
        public string TenNSP { get; set; }
    }
}
