using api_QLHH.Core.DTOs.Requests;
using api_QLHH.Core.DTOs.Responses;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Mapping
{
    public static class ChiTietKhoToChiTietKhoRequestDto
    {
        public static ChiTietKho TransformToEntity(ChiTietKhoRequestDto dto)
        {
            if (dto == null) return null!;

            return new ChiTietKho
            {
                KhoId = dto.KhoId,
                SanPhamId = dto.SanPhamId,
                SoLuong = dto.SoLuong,
            };
        }
    }
}
