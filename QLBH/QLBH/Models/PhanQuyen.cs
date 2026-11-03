using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QLBH.Models
{
    [PrimaryKey(nameof(MaQuyen), nameof(MaNV))]
    public class PhanQuyen
    {
        public byte MaQuyen { get; set; }
        public byte MaNV { get; set; }
    }
}
