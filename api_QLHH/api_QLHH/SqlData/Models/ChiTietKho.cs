using System.ComponentModel.DataAnnotations.Schema;

namespace api_QLHH.SqlData.Models
{
    public class ChiTietKho
    {
        [ForeignKey("SanPham")]
        public Guid SanPhamId { get; set; }

        [ForeignKey("Kho")]
        public Guid KhoId { get; set; }

        public int SoLuong { get; set; }

        public SanPham? SanPham { get; set; }
        public Kho? Kho { get; set; }
    }
}
