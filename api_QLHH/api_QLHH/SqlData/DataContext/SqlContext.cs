using Microsoft.EntityFrameworkCore;
using api_QLHH.SqlData.Models;

namespace api_QLHH.SqlData.DataContext
{
    public partial class SqlContext : DbContext
    {
        public SqlContext() { }

        public SqlContext(DbContextOptions<SqlContext> options)
            : base(options)
        {
        }

        // DBSet (mỗi bảng trong SQL là 1 DbSet)
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<NhaCungCap> NhaCungCap { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Kho> Kho { get; set; }
        public DbSet<DanhMucSanPham> DanhMucSanPham { get; set; }
        public DbSet<SanPham> SanPham { get; set; }
        public DbSet<PhieuNhap> PhieuNhap { get; set; }
        public DbSet<PhieuXuat> PhieuXuat { get; set; }

        // Các bảng có khóa chính kép
        public DbSet<ChiTietKho> ChiTietKho { get; set; }
        public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhap { get; set; }
        public DbSet<ChiTietPhieuXuat> ChiTietPhieuXuat { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChiTietKho>().HasKey(x => new { x.SanPhamId, x.KhoId });
            modelBuilder.Entity<ChiTietPhieuNhap>().HasKey(x => new { x.SanPhamId, x.PhieuNhapId });
            modelBuilder.Entity<ChiTietPhieuXuat>().HasKey(x => new { x.PhieuXuatId, x.SanPhamId });
            modelBuilder.Entity<ChiTietKho>().HasOne(ct => ct.SanPham).WithMany(sp => sp.ChiTietKhos).HasForeignKey(ct => ct.SanPhamId);
            modelBuilder.Entity<ChiTietKho>().HasOne(ct => ct.Kho).WithMany(k => k.ChiTietKhos).HasForeignKey(ct => ct.KhoId);
            modelBuilder.Entity<ChiTietPhieuNhap>().HasOne(ct => ct.PhieuNhap).WithMany(pn => pn.ChiTietPhieuNhaps).HasForeignKey(ct => ct.PhieuNhapId);
            modelBuilder.Entity<ChiTietPhieuNhap>().HasOne(ct => ct.SanPham).WithMany(sp => sp.ChiTietPhieuNhaps).HasForeignKey(ct => ct.SanPhamId);
            modelBuilder.Entity<ChiTietPhieuXuat>().HasOne(ct => ct.PhieuXuat).WithMany(px => px.ChiTietPhieuXuats).HasForeignKey(ct => ct.PhieuXuatId);
            modelBuilder.Entity<ChiTietPhieuXuat>().HasOne(ct => ct.SanPham).WithMany(sp => sp.ChiTietPhieuXuats).HasForeignKey(ct => ct.SanPhamId);
        }
    }
}
