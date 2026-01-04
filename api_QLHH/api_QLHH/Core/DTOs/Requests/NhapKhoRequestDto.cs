namespace api_QLHH.Core.DTOs.Requests
{
    public class NhapKhoRequestDto
    {
        public Guid KhoId { get; set; }
        public Guid SanPhamId { get; set; }
        public int SoLuong { get; set; }
    }
}
