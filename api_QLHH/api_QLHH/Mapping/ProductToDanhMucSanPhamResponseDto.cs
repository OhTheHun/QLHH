using api_QLHH.Core.DTOs.Responses;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Mapping
{
    public static class ProductToDanhMucSanPhamResponseDto
    {
        public static DanhMucSanPhamResponseDto TransForm(
            this DanhMucSanPham entity)
        {
            if (entity == null) return null!;

            return new DanhMucSanPhamResponseDto
            {
                DanhMucId = entity.DanhMucId,
                TenDanhMuc = entity.TenDanhMuc
            };
        }
    }
}
