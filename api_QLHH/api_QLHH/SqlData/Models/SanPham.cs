using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_QLHH.SqlData.Models
{
    public class SanPham
    {
        [Key]
        public Guid SanPhamId { get; set; }

        [ForeignKey("DanhMucSanPham")]
        public Guid DanhMucId { get; set; }

        public string TenSanPham { get; set; } = "";
        public string? HinhAnh { get; set; }
        public int DonGia { get; set; }
        public bool TrangThai { get; set; }

        public DanhMucSanPham? DanhMucSanPham { get; set; }

        public ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();
        public ICollection<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; } = new List<ChiTietPhieuXuat>();
        public ICollection<ChiTietKho>? ChiTietKhos { get; set; }
    }
}