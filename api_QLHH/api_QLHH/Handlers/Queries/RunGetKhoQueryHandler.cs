using api_QLHH.Core.DTOs.Responses;
using api_QLHH.Services.Interface;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Handlers.Queries
{
    public class RunGetKhoQueryHandler
    {
        private readonly IProductService _productService;
        public RunGetKhoQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<KhoResponseDto[]> Handle()
        {
            return await _productService.GetListKhoAsync();
        }

    }
}
