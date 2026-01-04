using api_QLHH.Core.DTOs.Responses;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Mapping
{
    public static class ChiTietKhoToChiTietKhoResponseDto
    {
        public static ChiTietKhoResponseDto Tranform(this ChiTietKho ct)
        {
            return new ChiTietKhoResponseDto
            {
                SanPhamId = ct.SanPhamId,
                TenSanPham = ct.SanPham != null
                                ? ct.SanPham.TenSanPham
                                : string.Empty,

                KhoId = ct.KhoId,
                TenKho = ct.Kho != null
                                ? ct.Kho.TenKho
                                : string.Empty,
                DiaChiKho = ct.Kho != null
                                ? ct.Kho.DiaChi
                                : string.Empty,
                SoLuong = ct.SoLuong
            };
        }
    }
}
