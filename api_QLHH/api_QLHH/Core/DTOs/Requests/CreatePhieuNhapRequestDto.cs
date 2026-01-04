namespace api_QLHH.Core.DTOs.Requests
{
    public class CreatePhieuNhapRequestDto
    {
        public Guid UserId { get; set; }
        public Guid NhaCungCapId { get; set; }
        public List<CreateChiTietPhieuNhapRequestDto> ChiTietPhieuNhap { get; set; } = [];
    }
}
