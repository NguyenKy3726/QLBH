using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    public class XaPhuong
    {
        [Key]
        public string MaXP { get; set; }
        public string TenXP { get; set; }
        public string MaTinh { get; set; }
    }
}
