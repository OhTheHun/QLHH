using System.ComponentModel.DataAnnotations.Schema;

namespace api_QLHH.SqlData.Models
{
    public class ChiTietPhieuNhap
    {
        [ForeignKey("SanPham")]
        public Guid SanPhamId { get; set; }

        [ForeignKey("PhieuNhap")]
        public Guid PhieuNhapId { get; set; }

        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public DateTime NgayNhap { get; set; }
        public SanPham? SanPham { get; set; }
        public PhieuNhap? PhieuNhap { get; set; }
    }
}
