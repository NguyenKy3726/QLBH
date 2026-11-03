using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    public class PhongBan
    {
        [Key]
        public byte MaPB { get; set; }
        public string TenPB { get; set; }
    }
}
