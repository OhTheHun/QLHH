using api_QLHH.Core.DTOs.Responses;
using api_QLHH.Services.Interface;

namespace api_QLHH.Handlers.Queries
{
    public class RunGetChiTietKhoQueryHandler
    {
        private readonly IProductService _productService;
        public RunGetChiTietKhoQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<ChiTietKhoResponseDto[]> Handle()
        {
            return await _productService.GetListChiTietKhoAsync();
        }
    }
}
