using api_QLHH.Core.DTOs.Requests;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Mapping
{
    public static class PhieuNhapToPhieuNhapRequestDto
    {
        public static PhieuNhap phieuNhapTransform(CreatePhieuNhapRequestDto request, Guid phieuNhapId)
        {
            return new PhieuNhap
            {
                PhieuNhapId = phieuNhapId,
                UserId = request.UserId,
                NhaCungCapId = request.NhaCungCapId,
                Code = $"PN{DateTime.Now:yyMMdd}{Guid.NewGuid().ToString("N")[..2].ToUpper()}"
            };
        }

        // goi IEnumerable lưu trữ 1 tập hợp vì có nhiều ChiTietPhieu trong 1 phieu nên cần vòng lặp để map từng đối tượng
        public static IEnumerable<ChiTietPhieuNhap> ChiTietPhieuNhapTransform(
            CreatePhieuNhapRequestDto request,
            Guid phieuNhapId)
        {
            return request.ChiTietPhieuNhap.Select(x => new ChiTietPhieuNhap
            {
                PhieuNhapId = phieuNhapId,
                SanPhamId = x.SanPhamId,
                SoLuong = x.SoLuong,
                DonGia = x.DonGia,
                NgayNhap = x.NgayNhap
            });
        }
    }
}
