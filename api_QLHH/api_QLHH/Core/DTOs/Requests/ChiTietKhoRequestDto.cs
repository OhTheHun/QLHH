namespace api_QLHH.Core.DTOs.Requests
{
    public class ChiTietKhoRequestDto
    {
        public Guid SanPhamId { get; set; }
        public Guid KhoId { get; set; }
        public int SoLuong { get; set; }
    }
}
