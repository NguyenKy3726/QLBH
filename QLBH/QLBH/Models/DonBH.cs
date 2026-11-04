using System;
using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    public class DonBH
    {
        [Key]
        [StringLength(11)]
        public string MaDBH { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        public string DCGH { get; set; }
        public string MaXP { get; set; }
        public byte? MaNV { get; set; }
        public short MaKH { get; set; }
    }
}
