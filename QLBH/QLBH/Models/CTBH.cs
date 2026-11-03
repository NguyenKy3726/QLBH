using Microsoft.EntityFrameworkCore;

namespace QLBH.Models
{
    [PrimaryKey(nameof(MaDBH), nameof(MaSP))]
    public class CTBH
    {
        public string MaDBH { get; set; }
        public string MaSP { get; set; }
        public byte SLB { get; set; }
        public decimal DGB { get; set; }
    }
}
