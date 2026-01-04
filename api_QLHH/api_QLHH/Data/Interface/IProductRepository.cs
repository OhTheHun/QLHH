using api_QLHH.Core.DTOs.Requests;
using api_QLHH.Core.DTOs.Responses;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Data.Interface
{
    public interface IProductRepository
    {
        Task<DanhMucSanPham[]> GetListDanhMucSanPhamAsync();
        Task<SanPham[]> GetSanPhamAsync();
        Task<Kho[]> GetListKhoAsync();
        Task<ChiTietKho[]> GetListChiTietKhoAsync();
        Task<LichSuNhapXuatResponseDto[]> GetLichSuNhapXuatAsync();
        Task AddPhieuNhapAsync(PhieuNhap phieuNhap);
        Task AddChiTietPhieuNhapAsync(IEnumerable<ChiTietPhieuNhap> chiTiet);
        Task SaveChangesAsync();
        Task<List<BaoCaoResponseDto>> GetBaoCaoAsync(DateTime fromDate,DateTime toDate);
        Task<DashBoardResponseDto> GetDashboardSummaryAsync(DateTime month);
        Task<List<SanPhamBanChayResponseDto>> GetTopProductsAsync(int top = 5);
        Task<List<SanPhamTonResponseDto>> GetLowStockProductsAsync();
        Task AddAsync(ChiTietKho chiTietKho);
        Task UpdateAsync(Guid SanPhamId, Guid KhoId, int SoLuong);
        Task DeleteChiTietKhoAsync(Guid khoId, Guid sanPhamId);
        Task<ChiTietKho?> GetChiTietKhoByIdAsync(Guid khoId, Guid sanPhamId);
        Task<SanPhamFollowingKhoResponseDto[]> GetSanPhamByKhoIdAsync(Guid khoId);
        Task UpdateChiTietKhoAsync(ChiTietKho chiTietKho);
        Task<ChiTietKho?> GetChiTietKhoAsync(Guid sanPhamId, Guid khoId);
    }
}
