using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    public class Tinh
    {
        [Key]
        public string MaTinh { get; set; }
        public string TenTinh { get; set; }
    }
}
