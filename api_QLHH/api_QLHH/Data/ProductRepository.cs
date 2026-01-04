using api_QLHH.Core.DTOs.Responses;
using api_QLHH.Data.Interface;
using api_QLHH.SqlData.DataContext;
using api_QLHH.SqlData.Models;
using Microsoft.EntityFrameworkCore;

namespace api_QLHH.Data
{
    public class ProductRepository(SqlContext dbContext): IProductRepository
    {
        private readonly SqlContext _dbContext = dbContext;

        public async Task AddChiTietPhieuNhapAsync(IEnumerable<ChiTietPhieuNhap> chiTiet)
        {
            await _dbContext.ChiTietPhieuNhap.AddRangeAsync(chiTiet);
        }

        public async Task AddPhieuNhapAsync(PhieuNhap phieuNhap)
        {
            await _dbContext.PhieuNhap.AddAsync(phieuNhap);
        }

        public async Task<List<BaoCaoResponseDto>> GetBaoCaoAsync(DateTime fromDate, DateTime toDate)
        {
            return await _dbContext.SanPham
            .Select(sp => new BaoCaoResponseDto
            {
                SanPhamId = sp.SanPhamId,
                TenSanPham = sp.TenSanPham,

                TongNhap = sp.ChiTietPhieuNhaps
                    .Where(n => n.NgayNhap >= fromDate && n.NgayNhap <= toDate) //query từ ChiTietPhieuNhap và xuất
                    .Sum(n => (int?)n.SoLuong) ?? 0,

                TongXuat = sp.ChiTietPhieuXuats
                    .Where(x => x.NgayGiao >= fromDate && x.NgayGiao <= toDate)
                    .Sum(x => (int?)x.SoLuong) ?? 0,

                TonCuoi =
                    (sp.ChiTietPhieuNhaps
                        .Where(n => n.NgayNhap >= fromDate && n.NgayNhap <= toDate)
                        .Sum(n => (int?)n.SoLuong) ?? 0)
                  -
                    (sp.ChiTietPhieuXuats
                        .Where(x => x.NgayGiao >= fromDate && x.NgayGiao <= toDate)
                        .Sum(x => (int?)x.SoLuong) ?? 0),

                DoanhThu = sp.ChiTietPhieuXuats
                    .Where(x => x.NgayGiao >= fromDate && x.NgayGiao <= toDate)
                    .Sum(x => (long?)x.SoLuong * x.DonGia) ?? 0
            })
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<LichSuNhapXuatResponseDto[]> GetLichSuNhapXuatAsync()
        {
            var nhapQuery = _dbContext.ChiTietPhieuNhap
                .AsNoTracking()
                .Select(ct => new LichSuNhapXuatResponseDto
                {
                    Id = ct.PhieuNhap!.PhieuNhapId,
                    SanPhamId = ct.SanPhamId,
                    Code = ct.PhieuNhap.Code,
                    TenSanPham = ct.SanPham!.TenSanPham,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia,
                    NgayGiao = DateOnly.FromDateTime(ct.NgayNhap),
                    Loai = "Nhap"
                });

            var xuatQuery = _dbContext.ChiTietPhieuXuat
                .AsNoTracking()
                .Select(ct => new LichSuNhapXuatResponseDto
                {
                    Id = ct.PhieuXuat!.PhieuXuatId,
                    SanPhamId = ct.SanPhamId,
                    Code = ct.PhieuXuat.Code,
                    TenSanPham = ct.SanPham!.TenSanPham,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia,
                    NgayGiao = DateOnly.FromDateTime(ct.NgayGiao),
                    Loai = "Xuat"
                });

            return await nhapQuery
                .Concat(xuatQuery)         
                .OrderByDescending(x => x.NgayGiao)
                .ToArrayAsync();
        }


        public async Task<ChiTietKho[]> GetListChiTietKhoAsync()
        {
            return await _dbContext.ChiTietKho
                .Include(x => x.SanPham)
                .Include(x => x.Kho)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<DanhMucSanPham[]> GetListDanhMucSanPhamAsync()
        {
            return await _dbContext.DanhMucSanPham.ToArrayAsync();
        }

        public async Task<Kho[]> GetListKhoAsync()
        {
           return await _dbContext.Kho.ToArrayAsync();
        }

        public async Task<SanPham[]> GetSanPhamAsync()
        {
            return await _dbContext.SanPham
                .Include(sp => sp.DanhMucSanPham)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task<DashBoardResponseDto> GetDashboardSummaryAsync(DateTime month)
        {
            var firstDay = new DateTime(month.Year, month.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            var tongDoanhThu = await _dbContext.ChiTietPhieuXuat
                .Where(x => x.NgayGiao >= firstDay && x.NgayGiao <= lastDay)
                .SumAsync(x => (long?)x.SoLuong * x.DonGia) ?? 0;

            var tongXuat = await _dbContext.ChiTietPhieuXuat
                .Where(x => x.NgayGiao >= firstDay && x.NgayGiao <= lastDay)
                .SumAsync(x => (int?)x.SoLuong) ?? 0;

            var tongNhap = await _dbContext.ChiTietPhieuNhap
                .Where(x => x.NgayNhap >= firstDay && x.NgayNhap <= lastDay)
                .SumAsync(x => (int?)x.SoLuong) ?? 0;

            var tongSanPham = await _dbContext.SanPham.CountAsync();

            return new DashBoardResponseDto
            {
                TongDoanhThu = tongDoanhThu,
                TongXuatHang = tongXuat,
                TongNhapHang = tongNhap,
                TongSanPham = tongSanPham
            };
        }

        public async Task<List<SanPhamBanChayResponseDto>> GetTopProductsAsync(int top = 5)
        {
            return await _dbContext.ChiTietPhieuXuat
                .GroupBy(x => x.SanPham.TenSanPham)
                .Select(g => new SanPhamBanChayResponseDto
                {
                    TenSanPham = g.Key,
                    SoLuong = g.Sum(x => x.SoLuong)
                })
                .OrderByDescending(x => x.SoLuong)
                .Take(top)
                .ToListAsync();
        }

        public async Task<List<SanPhamTonResponseDto>> GetLowStockProductsAsync()
        {
            return await _dbContext.ChiTietKho
            .GroupBy(x => x.SanPham.TenSanPham)    // nhóm lại theo tên sản phẩm 
            .Select(g => new SanPhamTonResponseDto
            {
                TenSanPham = g.Key,
                TonKho = g.Sum(x => x.SoLuong)        
            })
            .Where(x => x.TonKho <= 100)        
            .ToListAsync();
        }

        public async Task<ChiTietKho?> GetChiTietKhoByIdAsync(Guid khoId, Guid sanPhamId) =>
        await _dbContext.ChiTietKho
                      .Include(c => c.SanPham)
                      .Include(c => c.Kho)
                      .FirstOrDefaultAsync(c => c.KhoId == khoId && c.SanPhamId == sanPhamId);

        public async Task AddAsync(ChiTietKho chiTietKho)
        {
            _dbContext.ChiTietKho.Add(chiTietKho);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid khoId, Guid sanPhamId, int soLuong)
        {
            var entity = await _dbContext.ChiTietKho
                .FirstOrDefaultAsync(c => c.KhoId == khoId && c.SanPhamId == sanPhamId);
            if (entity == null)
                throw new Exception("Không tìm thấy chi tiết kho với khoId và sanPhamId tương ứng.");
            entity.SoLuong = soLuong;
            _dbContext.ChiTietKho.Update(entity); // chỉ update soLuong
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteChiTietKhoAsync(Guid khoId, Guid sanPhamId)
        {
            var entity = await GetChiTietKhoByIdAsync(khoId, sanPhamId);
            if (entity != null)
            {
                _dbContext.ChiTietKho.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<SanPhamFollowingKhoResponseDto[]> GetSanPhamByKhoIdAsync(Guid khoId)
        {
            return await _dbContext.ChiTietKho
                .AsNoTracking()
                .Where(x => x.KhoId == khoId && x.SoLuong > 0)
                .Select(x => new SanPhamFollowingKhoResponseDto
                {
                    SanPhamId = x.SanPhamId,
                    TenSanPham = x.SanPham!.TenSanPham,
                    SoLuong = x.SoLuong
                })
                .ToArrayAsync();
        }
        public async Task<ChiTietKho?> GetChiTietKhoAsync(Guid sanPhamId, Guid khoId)
        {
            return await _dbContext.ChiTietKho
                .FirstOrDefaultAsync(x => x.SanPhamId == sanPhamId && x.KhoId == khoId);
        }

        public async Task UpdateChiTietKhoAsync(ChiTietKho chiTietKho)
        {
             _dbContext.ChiTietKho.Update(chiTietKho);
        }
    }
}
