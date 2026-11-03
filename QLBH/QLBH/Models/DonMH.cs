using System;
using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    public class DonMH
    {
        [Key]
        public string MaDMH { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        public byte MaNV { get; set; }
        public short MaNCC { get; set; }
    }
}
