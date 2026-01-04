using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_QLHH.SqlData.Models
{
    public class PhieuNhap
    {
        [Key]
        public Guid PhieuNhapId { get; set; }

        [ForeignKey("Users")]
        public Guid UserId { get; set; }

        [ForeignKey("NhaCungCap")]
        public Guid NhaCungCapId { get; set; }
        public string Code { get; set; } = string.Empty;

        public Users? Users { get; set; }
        public NhaCungCap? NhaCungCap { get; set; }

        public ICollection<ChiTietPhieuNhap>? ChiTietPhieuNhaps { get; set; }
    }

}
