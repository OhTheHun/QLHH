using api_QLHH.Core.DTOs.Requests;
using api_QLHH.Core.DTOs.Responses;
using api_QLHH.Data.Interface;
using api_QLHH.Mapping;
using api_QLHH.Services.Interface;
using api_QLHH.SqlData.Models;
using Microsoft.EntityFrameworkCore;

namespace api_QLHH.Services
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;
        public async Task<DanhMucSanPhamResponseDto[]> GetDanhMucSanPhamsAsync()
        {
            var dmValues = await _productRepository.GetListDanhMucSanPhamAsync();
            return dmValues.Select(p => ProductToDanhMucSanPhamResponseDto.TransForm(p)).ToArray();
        }

        public async Task<LichSuNhapXuatResponseDto[]> GetLichSuNhapXuatAsync()
        {
            return await _productRepository.GetLichSuNhapXuatAsync();
        }

        public async Task<ChiTietKhoResponseDto[]> GetListChiTietKhoAsync()
        {
            var ctkValues = await _productRepository.GetListChiTietKhoAsync();
            return ctkValues.Select(p => ChiTietKhoToChiTietKhoResponseDto.Tranform(p)).ToArray();
        }

        public async Task<KhoResponseDto[]> GetListKhoAsync()
        {
            var khoValues = await _productRepository.GetListKhoAsync();
            return khoValues.Select(p => KhoToKhoResponseDto.TransForm(p)).ToArray();
        }

        public async Task<SanPhamResponseDTO[]> GetSanPhamAsync()
        {
            var spValues = await _productRepository.GetSanPhamAsync();
            return spValues.Select(p => SanPhamToSanPhamResponseDto.TransForm(p)).ToArray();
        }
        public async Task<Guid> CreatePhieuNhapAsync(CreatePhieuNhapRequestDto request)
        {
            if (!request.ChiTietPhieuNhap.Any())
                throw new Exception("Phiếu nhập không có chi tiết");

            var phieuNhapId = Guid.NewGuid();

            var phieuNhap = PhieuNhapToPhieuNhapRequestDto.phieuNhapTransform(request, phieuNhapId);
            var chiTietList = PhieuNhapToPhieuNhapRequestDto.ChiTietPhieuNhapTransform(request, phieuNhapId);

            await _productRepository.AddPhieuNhapAsync(phieuNhap);
            await _productRepository.AddChiTietPhieuNhapAsync(chiTietList);
            await _productRepository.SaveChangesAsync();

            return phieuNhapId;
        }
        public async Task<List<BaoCaoResponseDto>> GetBaoCaoAsync(DateTime fromDate,DateTime toDate)
        {
            if (fromDate > toDate)
                throw new ArgumentException("fromDate phải nhỏ hơn toDate");

            return await _productRepository.GetBaoCaoAsync(fromDate, toDate);
        }
        public async Task<DashBoardResponseDto> GetSummaryAsync(DateTime month)
        => await _productRepository.GetDashboardSummaryAsync(month);

        public async Task<List<SanPhamBanChayResponseDto>> GetTopProductsAsync()
            => await _productRepository.GetTopProductsAsync();

        public async Task<List<SanPhamTonResponseDto>> GetLowStockProductsAsync()
            => await _productRepository.GetLowStockProductsAsync();

        public async Task AddChiTietKhoAsync(ChiTietKhoRequestDto dto)
        {
            var entity = ChiTietKhoToChiTietKhoRequestDto.TransformToEntity(dto);
            await _productRepository.AddAsync(entity);
        }

        public async Task UpdateChiTietKhoAsync(ChiTietKhoRequestDto dto)
        {
            await _productRepository.UpdateAsync(dto.KhoId, dto.SanPhamId, dto.SoLuong);
        }
        public async Task DeleteChiTietKhoAsync(Guid khoId, Guid sanPhamId)
        {
            await _productRepository.DeleteChiTietKhoAsync(khoId, sanPhamId);
        }
        public async Task<ChiTietKhoRequestDto?> GetChiTietKhoByIdAsync(Guid khoId, Guid sanPhamId)
        {
            var ctk = await _productRepository.GetChiTietKhoByIdAsync(khoId, sanPhamId);
            if (ctk == null) return null;
            return new ChiTietKhoRequestDto
            {
                KhoId = ctk.KhoId,
                SanPhamId = ctk.SanPhamId,
                SoLuong = ctk.SoLuong
            };
        }

        public async Task<SanPhamFollowingKhoResponseDto[]> GetSanPhamByKhoIdAsync(Guid KhoId)
        {
            return await _productRepository.GetSanPhamByKhoIdAsync(KhoId);
        }

        public async Task CapNhatSoLuongKhoAsync(ChiTietKhoRequestDto[] ds)
        {
            foreach (var item in ds)
            {
                var chiTiet = await _productRepository.GetChiTietKhoAsync(item.SanPhamId, item.KhoId);
                if (chiTiet != null)
                {
                    chiTiet.SoLuong += item.SoLuong;
                    await _productRepository.UpdateChiTietKhoAsync(chiTiet);
                }
                else
                {
                    throw new Exception($"Chi tiết kho cho sản phẩm {item.SanPhamId} tại kho {item.KhoId} không tồn tại.");
                }

            }

            await _productRepository.SaveChangesAsync();
        }
    }
}
