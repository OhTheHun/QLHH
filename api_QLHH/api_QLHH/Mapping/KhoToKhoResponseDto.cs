using api_QLHH.Core.DTOs.Responses;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Mapping
{
    public static class KhoToKhoResponseDto
    {
        public static KhoResponseDto TransForm(
            this Kho entity)
        {
            if (entity == null) return null!;

            return new KhoResponseDto
            {
                KhoId = entity.KhoId,
                TenKho = entity.TenKho,
                DiaChi = entity.DiaChi,
            };
        }
    }
}
