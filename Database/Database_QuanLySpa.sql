USE master;
GO
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'QuanLySpa')
BEGIN
    ALTER DATABASE QuanLySpa SET SINGLE_USER WITH ROLLBACK IMMEDIATE; -- Đảm bảo không ai kết nối đến cơ sở dữ liệu
    DROP DATABASE QuanLySpa; -- Xóa cơ sở dữ liệu
END
GO
CREATE DATABASE QuanLySpa
GO
USE QuanLySpa
GO

-- Bảng lưu trữ vai trò người dùng (Khách hàng, Nhân viên, Quản lý)
CREATE TABLE VaiTro (
    IdVaiTro INT PRIMARY KEY IDENTITY(1,1),
    TenVaiTro NVARCHAR(50) NOT NULL UNIQUE -- 'KhachHang', 'NhanVien', 'QuanLy'
);

-- Bảng tài khoản chung cho việc đăng nhập
CREATE TABLE TaiKhoan (
    IdTaiKhoan INT PRIMARY KEY IDENTITY(1,1),
    TenDangNhap NVARCHAR(100) NOT NULL UNIQUE,
    MatKhauHash NVARCHAR(255) NOT NULL,
    IdVaiTro INT,
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP,
    TrangThai NVARCHAR(20) DEFAULT N'HoatDong' CHECK (TrangThai IN (N'HoatDong', N'Khoa')),
    FOREIGN KEY (IdVaiTro) REFERENCES VaiTro(IdVaiTro)
);

-- Bảng thông tin khách hàng
CREATE TABLE KhachHang (
    IdKhachHang INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100) NOT NULL,
    SoDienThoai NVARCHAR(15) UNIQUE,
    Email NVARCHAR(100) UNIQUE,
    DiaChi NVARCHAR(MAX),
    NgaySinh DATE,
    IdTaiKhoan INT UNIQUE,
    FOREIGN KEY (IdTaiKhoan) REFERENCES TaiKhoan(IdTaiKhoan) ON DELETE SET NULL
);

-- Bảng thông tin nhân viên
CREATE TABLE NhanVien (
    IdNhanVien INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100) NOT NULL,
    SoDienThoai NVARCHAR(15) UNIQUE,
    ChucVu NVARCHAR(100), -- Ví dụ: 'Chuyên viên chăm sóc da', 'Quản lý chi nhánh'
    IdTaiKhoan INT UNIQUE,
    FOREIGN KEY (IdTaiKhoan) REFERENCES TaiKhoan(IdTaiKhoan) ON DELETE SET NULL
);

-- Bảng danh mục sản phẩm
CREATE TABLE DanhMucSanPham (
    IdDanhMuc INT PRIMARY KEY IDENTITY(1,1),
    TenDanhMuc NVARCHAR(100) NOT NULL UNIQUE,
    MoTa NVARCHAR(MAX)
);

-- Bảng danh mục dịch vụ
CREATE TABLE DanhMucDichVu (
    IdDanhMuc INT PRIMARY KEY IDENTITY(1,1),
    TenDanhMuc NVARCHAR(100) NOT NULL UNIQUE,
    MoTa NVARCHAR(MAX)
);

-- Bảng sản phẩm
CREATE TABLE SanPham (
    IdSanPham INT PRIMARY KEY IDENTITY(1,1),
    TenSanPham NVARCHAR(255) NOT NULL,
    MoTa NVARCHAR(MAX),
    Gia DECIMAL(15, 2) NOT NULL,
    SoLuongTon INT DEFAULT 0,
    HinhAnhURL NVARCHAR(255),
    IdDanhMuc INT,
    FOREIGN KEY (IdDanhMuc) REFERENCES DanhMucSanPham(IdDanhMuc)
);

-- Bảng dịch vụ
CREATE TABLE DichVu (
    IdDichVu INT PRIMARY KEY IDENTITY(1,1),
    TenDichVu NVARCHAR(255) NOT NULL,
    MoTa NVARCHAR(MAX),
    Gia DECIMAL(15, 2) NOT NULL,
    ThoiLuongPhut INT,
    IdDanhMuc INT,
    FOREIGN KEY (IdDanhMuc) REFERENCES DanhMucDichVu(IdDanhMuc)
);

-- Bảng lịch làm việc của nhân viên
CREATE TABLE LichLamViec (
    IdLichLamViec INT PRIMARY KEY IDENTITY(1,1),
    IdNhanVien INT,
    ThoiGianBatDau DATETIME NOT NULL,
    ThoiGianKetThuc DATETIME NOT NULL,
    FOREIGN KEY (IdNhanVien) REFERENCES NhanVien(IdNhanVien) ON DELETE CASCADE
);

-- Bảng lịch hẹn của khách hàng
CREATE TABLE LichHen (
    IdLichHen INT PRIMARY KEY IDENTITY(1,1),
    IdKhachHang INT,
    IdDichVu INT,
    IdNhanVien INT,
    ThoiGianBatDau DATETIME NOT NULL,
    ThoiGianKetThuc DATETIME NOT NULL,
    TrangThai NVARCHAR(50) DEFAULT N'DaDat' CHECK (TrangThai IN (N'DaDat', N'DaHoanThanh', N'DaHuy', N'KhongDen', N'ChoThanhToan')),
    GhiChu NVARCHAR(MAX),
    FOREIGN KEY (IdKhachHang) REFERENCES KhachHang(IdKhachHang),
    FOREIGN KEY (IdDichVu) REFERENCES DichVu(IdDichVu),
    FOREIGN KEY (IdNhanVien) REFERENCES NhanVien(IdNhanVien)
);

-- Bảng chương trình khuyến mãi
CREATE TABLE KhuyenMai (
    IdKhuyenMai INT PRIMARY KEY IDENTITY(1,1),
    MaKhuyenMai NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(MAX) NOT NULL,
    PhanTramGiamGia DECIMAL(5, 2),
    SoTienGiamGia DECIMAL(15, 2),
    NgayBatDau DATE NOT NULL,
    NgayKetThuc DATE NOT NULL,
    ApDungCho NVARCHAR(20) DEFAULT N'TatCa' CHECK (ApDungCho IN (N'SanPham', N'DichVu', N'TatCa'))
);

-- Bảng đơn hàng (cho việc mua sản phẩm)
CREATE TABLE DonHang (
    IdDonHang INT PRIMARY KEY IDENTITY(1,1),
    IdKhachHang INT,
    IdNhanVien INT NULL,
    NgayDatHang DATETIME DEFAULT CURRENT_TIMESTAMP,
    TongTien DECIMAL(15, 2) NOT NULL,
    TrangThai NVARCHAR(50) DEFAULT N'ChoThanhToan' CHECK (TrangThai IN (N'ChoThanhToan', N'DangXuLy', N'DangGiao', N'DaGiao', N'DaHuy')),
    DiaChiGiaoHang NVARCHAR(MAX) NOT NULL,
    IdKhuyenMai INT,
    FOREIGN KEY (IdKhachHang) REFERENCES KhachHang(IdKhachHang),
    FOREIGN KEY (IdNhanVien) REFERENCES NhanVien(IdNhanVien) ON DELETE SET NULL,
    FOREIGN KEY (IdKhuyenMai) REFERENCES KhuyenMai(IdKhuyenMai) ON DELETE SET NULL
);

-- Bảng chi tiết đơn hàng
CREATE TABLE ChiTietDonHang (
    IdChiTietDonHang INT PRIMARY KEY IDENTITY(1,1),
    IdDonHang INT,
    IdSanPham INT,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(15, 2) NOT NULL,
    FOREIGN KEY (IdDonHang) REFERENCES DonHang(IdDonHang) ON DELETE CASCADE,
    FOREIGN KEY (IdSanPham) REFERENCES SanPham(IdSanPham)
);

-- Bảng quản lý thanh toán
CREATE TABLE ThanhToan (
    IdThanhToan INT PRIMARY KEY IDENTITY(1,1),
    IdDonHang INT NULL,
    IdLichHen INT NULL,
    SoTien DECIMAL(15, 2) NOT NULL,
    PhuongThucThanhToan NVARCHAR(100), -- 'Tiền Mặt', 'Chuyển Khoản', 'Thẻ Tín Dụng'
    ThoiGianThanhToan DATETIME DEFAULT CURRENT_TIMESTAMP,
    TrangThai NVARCHAR(50) DEFAULT N'ChuaThanhToan' CHECK (TrangThai IN (N'ChuaThanhToan', N'DaThanhToan', N'ThatBai')),
    MaGiaoDich NVARCHAR(255) NULL,
    FOREIGN KEY (IdDonHang) REFERENCES DonHang(IdDonHang),
    FOREIGN KEY (IdLichHen) REFERENCES LichHen(IdLichHen)
);

-- Bảng đánh giá của khách hàng
CREATE TABLE DanhGia (
    IdDanhGia INT PRIMARY KEY IDENTITY(1,1),
    IdKhachHang INT,
    IdSanPham INT NULL,
    IdDichVu INT NULL,
    SoSao INT NOT NULL CHECK (SoSao >= 1 AND SoSao <= 5),
    BinhLuan NVARCHAR(MAX),
    NgayDanhGia DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (IdKhachHang) REFERENCES KhachHang(IdKhachHang),
    FOREIGN KEY (IdSanPham) REFERENCES SanPham(IdSanPham),
    FOREIGN KEY (IdDichVu) REFERENCES DichVu(IdDichVu)
);

-- Bảng chat hỗ trợ
CREATE TABLE TinNhanHoTro (
    IdTinNhan INT PRIMARY KEY IDENTITY(1,1),
    IdNguoiGui INT,
    IdNguoiNhan INT,
    NoiDung NVARCHAR(MAX) NOT NULL,
    ThoiGianGui DATETIME DEFAULT CURRENT_TIMESTAMP,
    DaXem BIT DEFAULT 0, -- Sử dụng BIT cho True/False
    FOREIGN KEY (IdNguoiGui) REFERENCES TaiKhoan(IdTaiKhoan),
    FOREIGN KEY (IdNguoiNhan) REFERENCES TaiKhoan(IdTaiKhoan)
);

-- Chèn dữ liệu vai trò ban đầu
INSERT INTO VaiTro (TenVaiTro) VALUES (N'KhachHang'), (N'NhanVien'), (N'QuanLy');

-- =====================================================================
-- SAMPLE DATA INSERTION SCRIPT FOR SPA MANAGEMENT SYSTEM
-- =====================================================================

-- Lưu ý: Bảng VaiTro đã được chèn dữ liệu khi tạo bảng.
-- INSERT INTO VaiTro (TenVaiTro) VALUES (N'KhachHang'), (N'NhanVien'), (N'QuanLy');

-- 1. TẠO TÀI KHOẢN
-- Mật khẩu ở đây chỉ là chuỗi giả lập, trong thực tế bạn PHẢI băm (hash) mật khẩu.
INSERT INTO TaiKhoan (TenDangNhap, MatKhauHash, IdVaiTro) VALUES
(N'admin_spa', N'hashed_password_placeholder', 3), -- Quản lý (IdVaiTro=3)
(N'ngocanh.nv', N'hashed_password_placeholder', 2), -- Nhân viên (IdVaiTro=2)
(N'thanhhuyen.nv', N'hashed_password_placeholder', 2), -- Nhân viên
(N'minhthu.kh', N'hashed_password_placeholder', 1), -- Khách hàng (IdVaiTro=1)
(N'baotran.kh', N'hashed_password_placeholder', 1), -- Khách hàng
(N'quoctuan.kh', N'hashed_password_placeholder', 1); -- Khách hàng

-- 2. TẠO NHÂN VIÊN VÀ KHÁCH HÀNG (liên kết với tài khoản tương ứng)
INSERT INTO NhanVien (HoTen, SoDienThoai, ChucVu, IdTaiKhoan) VALUES
(N'Trần Ngọc Anh', '0987654321', N'Quản lý chi nhánh', 1),
(N'Nguyễn Ngọc Ánh', '0912345678', N'Chuyên viên chăm sóc da', 2),
(N'Lê Thanh Huyền', '0905112233', N'Chuyên viên trị liệu', 3);

INSERT INTO KhachHang (HoTen, SoDienThoai, Email, DiaChi, NgaySinh, IdTaiKhoan) VALUES
(N'Trần Thị Minh Thư', '0934567890', 'minhthu.tran@email.com', N'123 Đường ABC, Quận 1, TP. HCM', '1995-08-15', 4),
(N'Lý Bảo Trân', '0944556677', 'baotran.ly@email.com', N'456 Đường XYZ, Quận 3, TP. HCM', '1998-04-20', 5),
(N'Phạm Quốc Tuấn', '0966778899', 'quoctuan.pham@email.com', N'789 Đường DEF, Quận 7, TP. HCM', '1992-11-30', 6);

-- 3. TẠO DANH MỤC SẢN PHẨM VÀ DỊCH VỤ
INSERT INTO DanhMucSanPham (TenDanhMuc, MoTa) VALUES
(N'Chăm sóc da mặt', N'Các sản phẩm dành cho da mặt như sữa rửa mặt, toner, serum.'),
(N'Dưỡng thể', N'Sản phẩm chăm sóc da toàn thân.'),
(N'Chống nắng', N'Kem chống nắng bảo vệ da khỏi tia UV.');

INSERT INTO DanhMucDichVu (TenDanhMuc, MoTa) VALUES
(N'Massage trị liệu', N'Các liệu trình massage giúp thư giãn, giảm đau nhức.'),
(N'Chăm sóc da chuyên sâu', N'Các liệu trình làm sạch, tái tạo và phục hồi da.'),
(N'Triệt lông công nghệ cao', N'Dịch vụ triệt lông vĩnh viễn bằng công nghệ Laser Diode.');

-- 4. TẠO SẢN PHẨM VÀ DỊCH VỤ
INSERT INTO SanPham (TenSanPham, MoTa, Gia, SoLuongTon, HinhAnhURL, IdDanhMuc) VALUES
(N'Serum cấp ẩm Hyaluronic Acid', N'Serum giúp cấp ẩm sâu, cho làn da căng bóng.', 550000, 50, N'/images/serum-ha.jpg', 1),
(N'Kem chống nắng SPF 50+ PA++++', N'Bảo vệ da tối ưu, không gây bết dính.', 480000, 100, N'/images/kcn.jpg', 3),
(N'Sữa dưỡng thể Oliu', N'Dưỡng ẩm, làm mềm và mịn da toàn thân.', 350000, 75, N'/images/lotion-oliu.jpg', 2);

INSERT INTO DichVu (TenDichVu, MoTa, Gia, ThoiLuongPhut, IdDanhMuc) VALUES
(N'Massage đá nóng toàn thân', N'Sử dụng đá nóng bazan giúp giải tỏa căng thẳng, lưu thông khí huyết.', 600000, 90, 1),
(N'Chăm sóc da mặt chuyên sâu Aqua Peel', N'Làm sạch sâu lỗ chân lông, loại bỏ mụn cám, giúp da sáng mịn.', 850000, 75, 2),
(N'Triệt lông vùng nách', N'Liệu trình 10 buổi, bảo hành 5 năm.', 1500000, 30, 3);

-- 5. LÊN LỊCH LÀM VIỆC CHO NHÂN VIÊN
-- Giả sử hôm nay là ngày 7 tháng 7 năm 2025
INSERT INTO LichLamViec (IdNhanVien, ThoiGianBatDau, ThoiGianKetThuc) VALUES
(2, '2025-07-08 09:00:00', '2025-07-08 18:00:00'), -- NV Ngọc Ánh làm việc ngày 8/7
(3, '2025-07-08 13:00:00', '2025-07-08 21:00:00'); -- NV Thanh Huyền làm việc ngày 8/7

-- 6. KHÁCH HÀNG ĐẶT LỊCH HẸN
INSERT INTO LichHen (IdKhachHang, IdDichVu, IdNhanVien, ThoiGianBatDau, ThoiGianKetThuc, TrangThai, GhiChu) VALUES
(1, 2, 2, '2025-07-08 10:00:00', '2025-07-08 11:15:00', N'DaDat', N'Khách hàng có da nhạy cảm.'),
(2, 1, 3, '2025-07-08 14:00:00', '2025-07-08 15:30:00', N'DaDat', NULL);

-- 7. TẠO CHƯƠNG TRÌNH KHUYẾN MÃI
INSERT INTO KhuyenMai (MaKhuyenMai, MoTa, PhanTramGiamGia, NgayBatDau, NgayKetThuc, ApDungCho) VALUES
(N'HEVUIVẻ', N'Giảm giá 15% cho tất cả dịch vụ trong tháng 7', 15, '2025-07-01', '2025-07-31', N'DichVu');

-- 8. KHÁCH HÀNG MUA HÀNG
-- Tạo đơn hàng chính
INSERT INTO DonHang (IdKhachHang, IdNhanVien, TongTien, TrangThai, DiaChiGiaoHang) VALUES
(3, 2, 1030000, N'ChoThanhToan', N'789 Đường DEF, Quận 7, TP. HCM'); -- IdDonHang sẽ là 1

-- Thêm các sản phẩm vào chi tiết đơn hàng (cho đơn hàng có ID = 1)
INSERT INTO ChiTietDonHang (IdDonHang, IdSanPham, SoLuong, DonGia) VALUES
(1, 1, 1, 550000), -- Mua 1 Serum HA
(1, 2, 1, 480000); -- Mua 1 Kem chống nắng

-- 9. TẠO THANH TOÁN
-- Thanh toán cho đơn hàng sản phẩm (IdDonHang = 1)
INSERT INTO ThanhToan (IdDonHang, SoTien, PhuongThucThanhToan, TrangThai, MaGiaoDich) VALUES
(1, 1030000, N'Thẻ Tín Dụng', N'DaThanhToan', N'VNPAY_XYZ123');

-- Khách hàng đặt cọc cho lịch hẹn (IdLichHen = 2)
INSERT INTO ThanhToan (IdLichHen, SoTien, PhuongThucThanhToan, TrangThai) VALUES
(2, 200000, N'Chuyển Khoản', N'DaThanhToan');

-- 10. KHÁCH HÀNG ĐÁNH GIÁ
-- Giả sử lịch hẹn của khách Minh Thư (IdLichHen=1) đã hoàn thành và được cập nhật trạng thái
UPDATE LichHen SET TrangThai = N'DaHoanThanh' WHERE IdLichHen = 1;

-- Khách hàng Minh Thư để lại đánh giá cho dịch vụ đã sử dụng
INSERT INTO DanhGia (IdKhachHang, IdDichVu, SoSao, BinhLuan) VALUES
(1, 2, 5, N'Dịch vụ rất tốt, da mình cải thiện rõ rệt. Nhân viên Ngọc Ánh tay nghề cao và rất nhiệt tình.');

-- 11. TẠO TIN NHẮN HỖ TRỢ
-- Cuộc trò chuyện giữa khách Bảo Trân (IdTaiKhoan=5) và nhân viên Thanh Huyền (IdTaiKhoan=3)
INSERT INTO TinNhanHoTro (IdNguoiGui, IdNguoiNhan, NoiDung) VALUES
(5, 3, N'Chào bạn, mình muốn hỏi về liệu trình massage đá nóng.'),
(3, 5, N'Dạ chào bạn Bảo Trân, liệu trình này kéo dài 90 phút và rất tốt cho việc thư giãn cơ bắp ạ. Bạn có muốn đặt lịch không ạ?');