using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    public class LoaiSP
    {
        [Key]
        public short MaLSP { get; set; }
        public string TenLSP { get; set; }
        public byte MaNSP { get; set; }
    }
}
