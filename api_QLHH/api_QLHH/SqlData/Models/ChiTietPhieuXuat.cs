using System.ComponentModel.DataAnnotations.Schema;

namespace api_QLHH.SqlData.Models
{
    public class ChiTietPhieuXuat
    {
        [ForeignKey("PhieuXuat")]
        public Guid PhieuXuatId { get; set; }

        [ForeignKey("SanPham")]
        public Guid SanPhamId { get; set; }

        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public DateTime NgayGiao { get; set; }
        public string Code { get; set; } = "";

        public PhieuXuat PhieuXuat { get; set; } = new PhieuXuat();
        public SanPham SanPham { get; set; } = new SanPham();
    }
}
