namespace api_QLHH.Core.DTOs.Requests
{
    public class CreateChiTietPhieuNhapRequestDto
    {
        public Guid SanPhamId { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public DateTime NgayNhap { get; set; }
    }
}
