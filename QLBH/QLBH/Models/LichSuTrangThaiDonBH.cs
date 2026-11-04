using System;
using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    public class LichSuTrangThaiDonBH
    {
        [Key]
        public long MaLSTT { get; set; }
        public string MaDBH { get; set; } = string.Empty;
        public string? TrangThaiCu { get; set; }
        public string TrangThaiMoi { get; set; } = string.Empty;
        public DateTime ThoiDiem { get; set; }
        public byte? MaNV { get; set; }
        public string? GhiChu { get; set; }
    }
}
