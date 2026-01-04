using api_QLHH.Core.DTOs.Responses;
using api_QLHH.Services.Interface;

namespace api_QLHH.Handlers.Queries
{
    public class RunGetSanPhamQueryHandler
    {
        private readonly IProductService _productService;
        public RunGetSanPhamQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<SanPhamResponseDTO[]> Handle()
        {
            return await _productService.GetSanPhamAsync();
        }
    }
}
