namespace api_QLHH.Core.DTOs.Responses
{
    public class SanPhamResponseDTO
    {
        public Guid SanPhamId { get; set; }
        public Guid DanhMucId { get; set; }
        public string TenDanhMuc { get; set; } = "";
        public string TenSanPham { get; set; } = "";
        public string? HinhAnh { get; set; }
        public int DonGia { get; set; }
        public bool TrangThai { get; set; }
    }
}
