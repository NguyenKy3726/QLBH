using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    [PrimaryKey(nameof(MaDBH), nameof(MaSP))]
    public class CTBH
    {
        [StringLength(11)]
        public string MaDBH { get; set; }
        public string MaSP { get; set; }
        public byte SLB { get; set; }
        public decimal DGB { get; set; }
    }
}
