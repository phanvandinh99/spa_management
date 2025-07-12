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
        // TODO: Uncomment khi cần sử dụng TinNhanHoTro
        // public DbSet<TinNhanHoTro> TinNhanHoTro { get; set; }

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

            // TODO: Uncomment và cấu hình khi cần sử dụng TinNhanHoTro
            /*
            modelBuilder.Entity<TinNhanHoTro>(entity =>
            {
                entity.HasKey(e => e.IdTinNhan);
                entity.Property(e => e.NoiDung).IsRequired();
                entity.Property(e => e.DaXem).HasDefaultValue(false);
                
                // Cấu hình relationship với TaiKhoan
                entity.HasOne(e => e.NguoiGui)
                    .WithMany(e => e.TinNhanGui)
                    .HasForeignKey(e => e.IdNguoiGui)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.NguoiNhan)
                    .WithMany(e => e.TinNhanNhan)
                    .HasForeignKey(e => e.IdNguoiNhan)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            */

            modelBuilder.Entity<KhuyenMai>(entity =>
            {
                entity.HasKey(e => e.IdKhuyenMai);
                entity.Property(e => e.MaKhuyenMai).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MoTa).IsRequired();
                entity.Property(e => e.ApDungCho).HasDefaultValue("TatCa");
            });

            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.HasKey(e => e.IdDonHang);
                entity.Property(e => e.TongTien).HasColumnType("decimal(15,2)");
                entity.Property(e => e.TrangThai).HasDefaultValue("ChoThanhToan");
                entity.Property(e => e.DiaChiGiaoHang).IsRequired();
                
                entity.HasOne(e => e.KhachHang).WithMany(e => e.DonHangs).HasForeignKey(e => e.IdKhachHang);
                entity.HasOne(e => e.NhanVien).WithMany(e => e.DonHangs).HasForeignKey(e => e.IdNhanVien);
                entity.HasOne(e => e.KhuyenMai).WithMany(e => e.DonHangs).HasForeignKey(e => e.IdKhuyenMai);
            });

            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.HasKey(e => e.IdChiTietDonHang);
                entity.Property(e => e.SoLuong).IsRequired();
                entity.Property(e => e.DonGia).HasColumnType("decimal(15,2)");
                
                entity.HasOne(e => e.DonHang).WithMany(e => e.ChiTietDonHangs).HasForeignKey(e => e.IdDonHang);
                entity.HasOne(e => e.SanPham).WithMany(e => e.ChiTietDonHangs).HasForeignKey(e => e.IdSanPham);
            });

            modelBuilder.Entity<ThanhToan>(entity =>
            {
                entity.HasKey(e => e.IdThanhToan);
                entity.Property(e => e.SoTien).HasColumnType("decimal(15,2)");
                entity.Property(e => e.TrangThai).HasDefaultValue("ChuaThanhToan");
                
                entity.HasOne(e => e.DonHang).WithMany(e => e.ThanhToans).HasForeignKey(e => e.IdDonHang);
                entity.HasOne(e => e.LichHen).WithMany(e => e.ThanhToans).HasForeignKey(e => e.IdLichHen);
            });

            modelBuilder.Entity<DanhGia>(entity =>
            {
                entity.HasKey(e => e.IdDanhGia);
                entity.Property(e => e.SoSao).IsRequired();
                entity.Property(e => e.NgayDanhGia).HasDefaultValueSql("GETDATE()");
                
                entity.HasOne(e => e.KhachHang).WithMany(e => e.DanhGias).HasForeignKey(e => e.IdKhachHang);
                entity.HasOne(e => e.SanPham).WithMany(e => e.DanhGias).HasForeignKey(e => e.IdSanPham);
                entity.HasOne(e => e.DichVu).WithMany(e => e.DanhGias).HasForeignKey(e => e.IdDichVu);
            });

            modelBuilder.Entity<LichLamViec>(entity =>
            {
                entity.HasKey(e => e.IdLichLamViec);
                entity.Property(e => e.ThoiGianBatDau).IsRequired();
                entity.Property(e => e.ThoiGianKetThuc).IsRequired();
                
                entity.HasOne(e => e.NhanVien).WithMany(e => e.LichLamViecs).HasForeignKey(e => e.IdNhanVien);
            });

            modelBuilder.Entity<DanhMucSanPham>(entity =>
            {
                entity.HasKey(e => e.IdDanhMuc);
                entity.Property(e => e.TenDanhMuc).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.IdSanPham);
                entity.Property(e => e.TenSanPham).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Gia).HasColumnType("decimal(15,2)");
                entity.Property(e => e.SoLuongTon).HasDefaultValue(0);
                entity.Property(e => e.HinhAnhURL).HasMaxLength(255);
                
                entity.HasOne(e => e.DanhMucSanPham).WithMany(e => e.SanPhams).HasForeignKey(e => e.IdDanhMuc);
            });
        }
    }
} 