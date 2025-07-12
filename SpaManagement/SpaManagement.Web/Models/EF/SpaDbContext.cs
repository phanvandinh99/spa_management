using Microsoft.EntityFrameworkCore;

namespace SpaManagement.Web.Models.EF
{
    public class SpaDbContext : DbContext
    {
        public SpaDbContext(DbContextOptions<SpaDbContext> options) : base(options)
        {
        }

        // DbSet cho các bảng chính
        public DbSet<VaiTro> VaiTro { get; set; }
        public DbSet<TaiKhoan> TaiKhoan { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<DanhMucDichVu> DanhMucDichVu { get; set; }
        public DbSet<DichVu> DichVu { get; set; }
        public DbSet<LichHen> LichHen { get; set; }
        public DbSet<LichLamViec> LichLamViec { get; set; }
        public DbSet<DanhMucSanPham> DanhMucSanPham { get; set; }
        public DbSet<SanPham> SanPham { get; set; }
        public DbSet<KhuyenMai> KhuyenMai { get; set; }
        public DbSet<DonHang> DonHang { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
        public DbSet<ThanhToan> ThanhToan { get; set; }
        public DbSet<DanhGia> DanhGia { get; set; }
        public DbSet<TinNhanHoTro> TinNhanHoTro { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Cấu hình các entity và relationship
            modelBuilder.Entity<VaiTro>(entity =>
            {
                entity.HasKey(e => e.IdVaiTro);
                entity.Property(e => e.TenVaiTro).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.IdTaiKhoan);
                entity.Property(e => e.TenDangNhap).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MatKhauHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.TrangThai).HasDefaultValue("HoatDong");
                entity.HasOne(e => e.VaiTro).WithMany(e => e.TaiKhoans).HasForeignKey(e => e.IdVaiTro);
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.IdKhachHang);
                entity.Property(e => e.HoTen).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SoDienThoai).HasMaxLength(15);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.HasOne(e => e.TaiKhoan).WithOne(e => e.KhachHang).HasForeignKey<KhachHang>(e => e.IdTaiKhoan);
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.IdNhanVien);
                entity.Property(e => e.HoTen).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SoDienThoai).HasMaxLength(15);
                entity.Property(e => e.ChucVu).HasMaxLength(100);
                entity.HasOne(e => e.TaiKhoan).WithOne(e => e.NhanVien).HasForeignKey<NhanVien>(e => e.IdTaiKhoan);
            });

            modelBuilder.Entity<DanhMucDichVu>(entity =>
            {
                entity.HasKey(e => e.IdDanhMuc);
                entity.Property(e => e.TenDanhMuc).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<DichVu>(entity =>
            {
                entity.HasKey(e => e.IdDichVu);
                entity.Property(e => e.TenDichVu).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Gia).HasColumnType("decimal(15,2)");
                entity.HasOne(e => e.DanhMucDichVu).WithMany(e => e.DichVus).HasForeignKey(e => e.IdDanhMuc);
            });

            modelBuilder.Entity<LichHen>(entity =>
            {
                entity.HasKey(e => e.IdLichHen);
                entity.Property(e => e.TrangThai).HasDefaultValue("DaDat");
                entity.HasOne(e => e.KhachHang).WithMany(e => e.LichHens).HasForeignKey(e => e.IdKhachHang);
                entity.HasOne(e => e.DichVu).WithMany(e => e.LichHens).HasForeignKey(e => e.IdDichVu);
                entity.HasOne(e => e.NhanVien).WithMany(e => e.LichHens).HasForeignKey(e => e.IdNhanVien);
            });
        }
    }
} 