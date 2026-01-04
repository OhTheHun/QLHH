using api_QLHH.Core.DTOs.Requests;
using api_QLHH.Core.DTOs.Responses;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Services.Interface
{
    public interface IProductService
    {
        Task<DanhMucSanPhamResponseDto[]> GetDanhMucSanPhamsAsync();
        Task<SanPhamResponseDTO[]> GetSanPhamAsync();
        Task<KhoResponseDto[]> GetListKhoAsync();
        Task<ChiTietKhoResponseDto[]> GetListChiTietKhoAsync();
        Task<LichSuNhapXuatResponseDto[]> GetLichSuNhapXuatAsync();
        Task<Guid> CreatePhieuNhapAsync(CreatePhieuNhapRequestDto request);
        Task<List<BaoCaoResponseDto>> GetBaoCaoAsync(DateTime fromDate,DateTime toDate);
        Task<DashBoardResponseDto> GetSummaryAsync(DateTime month);
        Task<List<SanPhamBanChayResponseDto>> GetTopProductsAsync();
        Task<List<SanPhamTonResponseDto>> GetLowStockProductsAsync();
        Task<ChiTietKhoRequestDto?> GetChiTietKhoByIdAsync(Guid khoId, Guid sanPhamId);
        Task AddChiTietKhoAsync(ChiTietKhoRequestDto dto);
        Task UpdateChiTietKhoAsync(ChiTietKhoRequestDto dto);
        Task DeleteChiTietKhoAsync(Guid khoId, Guid sanPhamId);
        Task<SanPhamFollowingKhoResponseDto[]> GetSanPhamByKhoIdAsync(Guid KhoId);
        Task CapNhatSoLuongKhoAsync(ChiTietKhoRequestDto[] ds);



    }
}
