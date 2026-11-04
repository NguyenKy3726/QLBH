using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    [PrimaryKey(nameof(MaDMH), nameof(MaSP))]
    public class CTMH
    {
        [StringLength(11)]
        public string MaDMH { get; set; }
        public string MaSP { get; set; }
        public short SLM { get; set; }
        public decimal DGM { get; set; }
    }
}
