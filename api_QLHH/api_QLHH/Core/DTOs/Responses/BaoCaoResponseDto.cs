namespace api_QLHH.Core.DTOs.Responses
{
    public class BaoCaoResponseDto
    {
        public Guid SanPhamId { get; set; }
        public string TenSanPham { get; set; } = string.Empty;

        public int TongNhap { get; set; }
        public int TongXuat { get; set; }
        public int TonCuoi { get; set; }

        public long DoanhThu { get; set; }
    }
}
