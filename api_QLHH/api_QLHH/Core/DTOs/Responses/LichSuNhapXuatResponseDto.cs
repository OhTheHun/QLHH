namespace api_QLHH.Core.DTOs.Responses
{
    public class LichSuNhapXuatResponseDto
    {
        public Guid Id { get; set; }
        public Guid SanPhamId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string TenSanPham { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public DateOnly NgayGiao { get; set; }
        public string Loai { get; set; } = string.Empty;

    }
}
