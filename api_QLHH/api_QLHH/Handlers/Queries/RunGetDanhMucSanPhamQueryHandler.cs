using api_QLHH.Core.DTOs.Responses;
using api_QLHH.Services.Interface;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Handlers.Queries
{
    public class RunGetDanhMucSanPhamQueryHandler
    {
        private readonly IProductService _productService;
        public RunGetDanhMucSanPhamQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<DanhMucSanPhamResponseDto[] > Handle()
        {
            return await _productService.GetDanhMucSanPhamsAsync();
        }
    }
}
