namespace api_QLHH.Core.DTOs.Responses
{
    public class SanPhamFollowingKhoResponseDto
    {
        public Guid SanPhamId { get; set; }
        public string TenSanPham { get; set; } = "";
        public int SoLuong { get; set; }

    }
}
