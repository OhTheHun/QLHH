using api_QLHH.Core.DTOs.Responses;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Mapping
{
    public static class SanPhamToSanPhamResponseDto
    {
        public static SanPhamResponseDTO TransForm (this SanPham sp)
        {
            return new SanPhamResponseDTO
            {
                SanPhamId = sp.SanPhamId,
                DanhMucId = sp.DanhMucId,
                TenDanhMuc = sp.DanhMucSanPham != null
                                ? sp.DanhMucSanPham.TenDanhMuc
                                : string.Empty,
                TenSanPham = sp.TenSanPham,
                HinhAnh = sp.HinhAnh,
                DonGia = sp.DonGia,
                TrangThai = sp.TrangThai
            };
        }
    }
}
