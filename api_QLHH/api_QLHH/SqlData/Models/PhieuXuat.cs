using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_QLHH.SqlData.Models
{
    public class PhieuXuat
    {
        [Key]
        public Guid PhieuXuatId { get; set; }

        [ForeignKey("Users")]
        public Guid UserId { get; set; }

        [ForeignKey("KhachHang")]
        public Guid KhachHangId { get; set; }
        public string Code { get; set; } = string.Empty;

        public Users? Users { get; set; }
        public KhachHang? KhachHang { get; set; }

        public ICollection<ChiTietPhieuXuat>? ChiTietPhieuXuats { get; set; }
    }
}
