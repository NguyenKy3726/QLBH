using Microsoft.EntityFrameworkCore;

namespace QLBH.Models
{
    [PrimaryKey(nameof(MaDMH), nameof(MaSP))]
    public class CTMH
    {
        public string MaDMH { get; set; }
        public string MaSP { get; set; }
        public short SLM { get; set; }
        public decimal DGM { get; set; }
    }
}
