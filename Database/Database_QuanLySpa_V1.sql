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
/****** Object:  Table [dbo].[ChiTietDonHang]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietDonHang](
	[IdChiTietDonHang] [int] IDENTITY(1,1) NOT NULL,
	[IdDonHang] [int] NULL,
	[IdSanPham] [int] NULL,
	[SoLuong] [int] NOT NULL,
	[DonGia] [decimal](15, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdChiTietDonHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DanhGia]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhGia](
	[IdDanhGia] [int] IDENTITY(1,1) NOT NULL,
	[IdKhachHang] [int] NULL,
	[IdSanPham] [int] NULL,
	[IdDichVu] [int] NULL,
	[SoSao] [int] NOT NULL,
	[BinhLuan] [nvarchar](max) NULL,
	[NgayDanhGia] [datetime] NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[IdDanhGia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DanhMucDichVu]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhMucDichVu](
	[IdDanhMuc] [int] IDENTITY(1,1) NOT NULL,
	[TenDanhMuc] [nvarchar](100) NOT NULL,
	[MoTa] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDanhMuc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DanhMucSanPham]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhMucSanPham](
	[IdDanhMuc] [int] IDENTITY(1,1) NOT NULL,
	[TenDanhMuc] [nvarchar](100) NOT NULL,
	[MoTa] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDanhMuc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DichVu]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DichVu](
	[IdDichVu] [int] IDENTITY(1,1) NOT NULL,
	[TenDichVu] [nvarchar](255) NOT NULL,
	[MoTa] [nvarchar](max) NULL,
	[Gia] [decimal](15, 2) NOT NULL,
	[ThoiLuongPhut] [int] NULL,
	[IdDanhMuc] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDichVu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DonHang]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonHang](
	[IdDonHang] [int] IDENTITY(1,1) NOT NULL,
	[IdKhachHang] [int] NULL,
	[IdNhanVien] [int] NULL,
	[NgayDatHang] [datetime] NULL DEFAULT (getdate()),
	[TongTien] [decimal](15, 2) NOT NULL,
	[TrangThai] [nvarchar](50) NULL DEFAULT (N'ChoThanhToan'),
	[DiaChiGiaoHang] [nvarchar](max) NOT NULL,
	[IdKhuyenMai] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDonHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KhachHang]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhachHang](
	[IdKhachHang] [int] IDENTITY(1,1) NOT NULL,
	[HoTen] [nvarchar](100) NOT NULL,
	[SoDienThoai] [nvarchar](15) NULL,
	[Email] [nvarchar](100) NULL,
	[DiaChi] [nvarchar](max) NULL,
	[NgaySinh] [date] NULL,
	[IdTaiKhoan] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdKhachHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KhuyenMai]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhuyenMai](
	[IdKhuyenMai] [int] IDENTITY(1,1) NOT NULL,
	[MaKhuyenMai] [nvarchar](50) NOT NULL,
	[MoTa] [nvarchar](max) NOT NULL,
	[PhanTramGiamGia] [decimal](5, 2) NULL,
	[SoTienGiamGia] [decimal](15, 2) NULL,
	[NgayBatDau] [date] NOT NULL,
	[NgayKetThuc] [date] NOT NULL,
	[ApDungCho] [nvarchar](20) NULL DEFAULT (N'TatCa'),
PRIMARY KEY CLUSTERED 
(
	[IdKhuyenMai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LichHen]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LichHen](
	[IdLichHen] [int] IDENTITY(1,1) NOT NULL,
	[IdKhachHang] [int] NULL,
	[IdDichVu] [int] NULL,
	[IdNhanVien] [int] NULL,
	[ThoiGianBatDau] [datetime] NOT NULL,
	[ThoiGianKetThuc] [datetime] NOT NULL,
	[TrangThai] [nvarchar](50) NULL DEFAULT (N'DaDat'),
	[GhiChu] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdLichHen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LichLamViec]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LichLamViec](
	[IdLichLamViec] [int] IDENTITY(1,1) NOT NULL,
	[IdNhanVien] [int] NULL,
	[ThoiGianBatDau] [datetime] NOT NULL,
	[ThoiGianKetThuc] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdLichLamViec] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NhanVien]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien](
	[IdNhanVien] [int] IDENTITY(1,1) NOT NULL,
	[HoTen] [nvarchar](100) NOT NULL,
	[SoDienThoai] [nvarchar](15) NULL,
	[ChucVu] [nvarchar](100) NULL,
	[IdTaiKhoan] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdNhanVien] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SanPham]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanPham](
	[IdSanPham] [int] IDENTITY(1,1) NOT NULL,
	[TenSanPham] [nvarchar](255) NOT NULL,
	[MoTa] [nvarchar](max) NULL,
	[Gia] [decimal](15, 2) NOT NULL,
	[SoLuongTon] [int] NULL DEFAULT ((0)),
	[HinhAnhURL] [nvarchar](255) NULL,
	[IdDanhMuc] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdSanPham] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[IdTaiKhoan] [int] IDENTITY(1,1) NOT NULL,
	[TenDangNhap] [nvarchar](100) NOT NULL,
	[MatKhauHash] [nvarchar](255) NOT NULL,
	[IdVaiTro] [int] NULL,
	[NgayTao] [datetime] NULL DEFAULT (getdate()),
	[TrangThai] [nvarchar](20) NULL DEFAULT (N'HoatDong'),
PRIMARY KEY CLUSTERED 
(
	[IdTaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ThanhToan]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThanhToan](
	[IdThanhToan] [int] IDENTITY(1,1) NOT NULL,
	[IdDonHang] [int] NULL,
	[IdLichHen] [int] NULL,
	[SoTien] [decimal](15, 2) NOT NULL,
	[PhuongThucThanhToan] [nvarchar](100) NULL,
	[ThoiGianThanhToan] [datetime] NULL DEFAULT (getdate()),
	[TrangThai] [nvarchar](50) NULL DEFAULT (N'ChuaThanhToan'),
	[MaGiaoDich] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdThanhToan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TinNhanHoTro]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TinNhanHoTro](
	[IdTinNhan] [int] IDENTITY(1,1) NOT NULL,
	[IdNguoiGui] [int] NULL,
	[IdNguoiNhan] [int] NULL,
	[NoiDung] [nvarchar](max) NOT NULL,
	[ThoiGianGui] [datetime] NULL DEFAULT (getdate()),
	[DaXem] [bit] NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[IdTinNhan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VaiTro]    Script Date: 20/07/2025 23:52:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VaiTro](
	[IdVaiTro] [int] IDENTITY(1,1) NOT NULL,
	[TenVaiTro] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdVaiTro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[ChiTietDonHang] ON 

INSERT [dbo].[ChiTietDonHang] ([IdChiTietDonHang], [IdDonHang], [IdSanPham], [SoLuong], [DonGia]) VALUES (1, 1, 1, 1, CAST(550000.00 AS Decimal(15, 2)))
INSERT [dbo].[ChiTietDonHang] ([IdChiTietDonHang], [IdDonHang], [IdSanPham], [SoLuong], [DonGia]) VALUES (2, 1, 2, 1, CAST(480000.00 AS Decimal(15, 2)))
INSERT [dbo].[ChiTietDonHang] ([IdChiTietDonHang], [IdDonHang], [IdSanPham], [SoLuong], [DonGia]) VALUES (3, 2, 3, 1, CAST(350000.00 AS Decimal(15, 2)))
INSERT [dbo].[ChiTietDonHang] ([IdChiTietDonHang], [IdDonHang], [IdSanPham], [SoLuong], [DonGia]) VALUES (4, 3, 2, 100, CAST(480000.00 AS Decimal(15, 2)))
INSERT [dbo].[ChiTietDonHang] ([IdChiTietDonHang], [IdDonHang], [IdSanPham], [SoLuong], [DonGia]) VALUES (5, 4, 1, 1, CAST(300000.00 AS Decimal(15, 2)))
INSERT [dbo].[ChiTietDonHang] ([IdChiTietDonHang], [IdDonHang], [IdSanPham], [SoLuong], [DonGia]) VALUES (6, 5, 6, 1, CAST(600000.00 AS Decimal(15, 2)))
SET IDENTITY_INSERT [dbo].[ChiTietDonHang] OFF
SET IDENTITY_INSERT [dbo].[DanhGia] ON 

INSERT [dbo].[DanhGia] ([IdDanhGia], [IdKhachHang], [IdSanPham], [IdDichVu], [SoSao], [BinhLuan], [NgayDanhGia]) VALUES (1, 1, NULL, 2, 5, N'Dịch vụ rất tốt, da mình cải thiện rõ rệt. Nhân viên Ngọc Ánh tay nghề cao và rất nhiệt tình.', CAST(N'2025-07-12 22:21:44.250' AS DateTime))
SET IDENTITY_INSERT [dbo].[DanhGia] OFF
SET IDENTITY_INSERT [dbo].[DanhMucDichVu] ON 

INSERT [dbo].[DanhMucDichVu] ([IdDanhMuc], [TenDanhMuc], [MoTa]) VALUES (1, N'Massage trị liệu', N'Các liệu trình massage giúp thư giãn, giảm đau nhức.')
INSERT [dbo].[DanhMucDichVu] ([IdDanhMuc], [TenDanhMuc], [MoTa]) VALUES (2, N'Chăm sóc da chuyên sâu', N'Các liệu trình làm sạch, tái tạo và phục hồi da.')
INSERT [dbo].[DanhMucDichVu] ([IdDanhMuc], [TenDanhMuc], [MoTa]) VALUES (3, N'Triệt lông công nghệ cao', N'Dịch vụ triệt lông vĩnh viễn bằng công nghệ Laser Diode.')
SET IDENTITY_INSERT [dbo].[DanhMucDichVu] OFF
SET IDENTITY_INSERT [dbo].[DanhMucSanPham] ON 

INSERT [dbo].[DanhMucSanPham] ([IdDanhMuc], [TenDanhMuc], [MoTa]) VALUES (1, N'Chăm sóc da mặt', N'Các sản phẩm dành cho da mặt như sữa rửa mặt, toner, serum.')
INSERT [dbo].[DanhMucSanPham] ([IdDanhMuc], [TenDanhMuc], [MoTa]) VALUES (2, N'Dưỡng thể', N'Sản phẩm chăm sóc da toàn thân.')
INSERT [dbo].[DanhMucSanPham] ([IdDanhMuc], [TenDanhMuc], [MoTa]) VALUES (3, N'Chống nắng', N'Kem chống nắng bảo vệ da khỏi tia UV.')
SET IDENTITY_INSERT [dbo].[DanhMucSanPham] OFF
SET IDENTITY_INSERT [dbo].[DichVu] ON 

INSERT [dbo].[DichVu] ([IdDichVu], [TenDichVu], [MoTa], [Gia], [ThoiLuongPhut], [IdDanhMuc]) VALUES (1, N'Phun Màu Thẩm Mỹ', N'Phun xăm thẩm mỹ gồm các dịch vụ chính: Phun chân mày, Phun môi, Phun mí, Xóa sửa mày môi mí làm ở chỗ khác bị hư', CAST(70000.00 AS Decimal(15, 2)), 90, 2)
INSERT [dbo].[DichVu] ([IdDichVu], [TenDichVu], [MoTa], [Gia], [ThoiLuongPhut], [IdDanhMuc]) VALUES (2, N'Massage Đắp Lá Thuốc Đông Y', N'Đây liệu pháp trị liệu cột sống thắt lưng bằng các phương pháp bấm huyệt, xoa bóp, cứu huyệt sử dụng lực từ bàn tay, ngón tay để day - ấn - bấm - xoa, kết hợp trị liệu hồng quang, thoa cao nóng, điện xung tại các huyệt đạo vùng cột sống tắt lưng,… để chữa những trường hợp đau mỏi cột sống, hạn chế vận động cột sống thắt lưng.', CAST(80000.00 AS Decimal(15, 2)), 60, 1)
INSERT [dbo].[DichVu] ([IdDichVu], [TenDichVu], [MoTa], [Gia], [ThoiLuongPhut], [IdDanhMuc]) VALUES (4, N'Triệt Lông Toàn Thân', N'Triệt lông toàn bộ cơ thể với chu kỳ 3 lần/tuần trong vòng 1 tháng liên tiếp. Quý khách sẽ được bảo hành trọn đời nếu lông mọc lại', CAST(1000000.00 AS Decimal(15, 2)), 120, 3)
INSERT [dbo].[DichVu] ([IdDichVu], [TenDichVu], [MoTa], [Gia], [ThoiLuongPhut], [IdDanhMuc]) VALUES (5, N'Massage Cổ Vai Gáy', N'Bạn được thư giản cổ vai gáy với đội ngũ nhân viên lâu năm, giàu kinh nghiệm... ', CAST(120000.00 AS Decimal(15, 2)), 45, 1)
INSERT [dbo].[DichVu] ([IdDichVu], [TenDichVu], [MoTa], [Gia], [ThoiLuongPhut], [IdDanhMuc]) VALUES (6, N'Massage Cổ Chân Thuốc Bắc', N'Bạn sẽ được massage thảo mộc quý thải độc bàn chân', CAST(20000.00 AS Decimal(15, 2)), 20, 1)
SET IDENTITY_INSERT [dbo].[DichVu] OFF
SET IDENTITY_INSERT [dbo].[DonHang] ON 

INSERT [dbo].[DonHang] ([IdDonHang], [IdKhachHang], [IdNhanVien], [NgayDatHang], [TongTien], [TrangThai], [DiaChiGiaoHang], [IdKhuyenMai]) VALUES (1, 3, 2, CAST(N'2025-07-12 22:21:44.240' AS DateTime), CAST(1030000.00 AS Decimal(15, 2)), N'DangXuLy', N'789 Đường DEF, Quận 7, TP. HCM', NULL)
INSERT [dbo].[DonHang] ([IdDonHang], [IdKhachHang], [IdNhanVien], [NgayDatHang], [TongTien], [TrangThai], [DiaChiGiaoHang], [IdKhuyenMai]) VALUES (2, 4, NULL, CAST(N'2025-07-13 15:55:31.690' AS DateTime), CAST(350000.00 AS Decimal(15, 2)), N'DaGiao', N'ádasd', NULL)
INSERT [dbo].[DonHang] ([IdDonHang], [IdKhachHang], [IdNhanVien], [NgayDatHang], [TongTien], [TrangThai], [DiaChiGiaoHang], [IdKhuyenMai]) VALUES (3, 5, NULL, CAST(N'2025-07-13 23:24:01.280' AS DateTime), CAST(48000000.00 AS Decimal(15, 2)), N'DaHuy', N'asd asd ádasd', NULL)
INSERT [dbo].[DonHang] ([IdDonHang], [IdKhachHang], [IdNhanVien], [NgayDatHang], [TongTien], [TrangThai], [DiaChiGiaoHang], [IdKhuyenMai]) VALUES (4, 4, NULL, CAST(N'2025-07-20 23:31:11.147' AS DateTime), CAST(300000.00 AS Decimal(15, 2)), N'ChoThanhToan', N'ABC', NULL)
INSERT [dbo].[DonHang] ([IdDonHang], [IdKhachHang], [IdNhanVien], [NgayDatHang], [TongTien], [TrangThai], [DiaChiGiaoHang], [IdKhuyenMai]) VALUES (5, 4, NULL, CAST(N'2025-07-20 23:36:56.177' AS DateTime), CAST(600000.00 AS Decimal(15, 2)), N'DangGiao', N'test', NULL)
SET IDENTITY_INSERT [dbo].[DonHang] OFF
SET IDENTITY_INSERT [dbo].[KhachHang] ON 

INSERT [dbo].[KhachHang] ([IdKhachHang], [HoTen], [SoDienThoai], [Email], [DiaChi], [NgaySinh], [IdTaiKhoan]) VALUES (1, N'Trần Thị Minh Thư', N'0934567890', N'minhthu.tran@email.com', N'123 Đường ABC, Quận 1, TP. HCM', CAST(N'1995-08-15' AS Date), 4)
INSERT [dbo].[KhachHang] ([IdKhachHang], [HoTen], [SoDienThoai], [Email], [DiaChi], [NgaySinh], [IdTaiKhoan]) VALUES (2, N'Lý Bảo Trân', N'0944556677', N'baotran.ly@email.com', N'456 Đường XYZ, Quận 3, TP. HCM', CAST(N'1998-04-20' AS Date), 5)
INSERT [dbo].[KhachHang] ([IdKhachHang], [HoTen], [SoDienThoai], [Email], [DiaChi], [NgaySinh], [IdTaiKhoan]) VALUES (3, N'Phạm Quốc Tuấn', N'0966778899', N'quoctuan.pham@email.com', N'789 Đường DEF, Quận 7, TP. HCM', CAST(N'1992-11-30' AS Date), 6)
INSERT [dbo].[KhachHang] ([IdKhachHang], [HoTen], [SoDienThoai], [Email], [DiaChi], [NgaySinh], [IdTaiKhoan]) VALUES (4, N'Phan Định', N'0971010282', N'Dinh@gmail.com', NULL, NULL, 7)
INSERT [dbo].[KhachHang] ([IdKhachHang], [HoTen], [SoDienThoai], [Email], [DiaChi], [NgaySinh], [IdTaiKhoan]) VALUES (5, N'Test Phan', N'0971010281', N'Dinh1@gmail.com', NULL, NULL, 8)
SET IDENTITY_INSERT [dbo].[KhachHang] OFF
SET IDENTITY_INSERT [dbo].[KhuyenMai] ON 

INSERT [dbo].[KhuyenMai] ([IdKhuyenMai], [MaKhuyenMai], [MoTa], [PhanTramGiamGia], [SoTienGiamGia], [NgayBatDau], [NgayKetThuc], [ApDungCho]) VALUES (1, N'HEVUIVẻ', N'Giảm giá 15% cho tất cả dịch vụ trong tháng 7', CAST(15.00 AS Decimal(5, 2)), NULL, CAST(N'2025-07-01' AS Date), CAST(N'2025-07-31' AS Date), N'DichVu')
SET IDENTITY_INSERT [dbo].[KhuyenMai] OFF
SET IDENTITY_INSERT [dbo].[LichHen] ON 

INSERT [dbo].[LichHen] ([IdLichHen], [IdKhachHang], [IdDichVu], [IdNhanVien], [ThoiGianBatDau], [ThoiGianKetThuc], [TrangThai], [GhiChu]) VALUES (1, 1, 2, 2, CAST(N'2025-07-08 10:00:00.000' AS DateTime), CAST(N'2025-07-08 11:15:00.000' AS DateTime), N'DaHoanThanh', N'Khách hàng có da nhạy cảm.')
INSERT [dbo].[LichHen] ([IdLichHen], [IdKhachHang], [IdDichVu], [IdNhanVien], [ThoiGianBatDau], [ThoiGianKetThuc], [TrangThai], [GhiChu]) VALUES (2, 2, 1, 3, CAST(N'2025-07-08 14:00:00.000' AS DateTime), CAST(N'2025-07-08 15:30:00.000' AS DateTime), N'DaDat', NULL)
INSERT [dbo].[LichHen] ([IdLichHen], [IdKhachHang], [IdDichVu], [IdNhanVien], [ThoiGianBatDau], [ThoiGianKetThuc], [TrangThai], [GhiChu]) VALUES (3, 4, 1, 1, CAST(N'2025-07-19 15:23:00.000' AS DateTime), CAST(N'2025-07-19 16:53:00.000' AS DateTime), N'DaHuy', N'Abxc ')
INSERT [dbo].[LichHen] ([IdLichHen], [IdKhachHang], [IdDichVu], [IdNhanVien], [ThoiGianBatDau], [ThoiGianKetThuc], [TrangThai], [GhiChu]) VALUES (4, 4, 2, 1, CAST(N'2025-07-13 15:41:00.000' AS DateTime), CAST(N'2025-07-13 16:56:00.000' AS DateTime), N'DaDat', N'Test')
INSERT [dbo].[LichHen] ([IdLichHen], [IdKhachHang], [IdDichVu], [IdNhanVien], [ThoiGianBatDau], [ThoiGianKetThuc], [TrangThai], [GhiChu]) VALUES (5, 5, 1, 1, CAST(N'2025-07-13 23:22:00.000' AS DateTime), CAST(N'2025-07-14 00:52:00.000' AS DateTime), N'DaDat', N'Tôi muốn massage đá nóng nè')
INSERT [dbo].[LichHen] ([IdLichHen], [IdKhachHang], [IdDichVu], [IdNhanVien], [ThoiGianBatDau], [ThoiGianKetThuc], [TrangThai], [GhiChu]) VALUES (6, 5, 5, NULL, CAST(N'2025-07-13 23:33:00.000' AS DateTime), CAST(N'2025-07-14 00:33:00.000' AS DateTime), N'DaDat', NULL)
INSERT [dbo].[LichHen] ([IdLichHen], [IdKhachHang], [IdDichVu], [IdNhanVien], [ThoiGianBatDau], [ThoiGianKetThuc], [TrangThai], [GhiChu]) VALUES (7, 5, 5, 1, CAST(N'2025-07-14 12:36:00.000' AS DateTime), CAST(N'2025-07-14 13:36:00.000' AS DateTime), N'DaHuy', N'ÁDASD')
SET IDENTITY_INSERT [dbo].[LichHen] OFF
SET IDENTITY_INSERT [dbo].[LichLamViec] ON 

INSERT [dbo].[LichLamViec] ([IdLichLamViec], [IdNhanVien], [ThoiGianBatDau], [ThoiGianKetThuc]) VALUES (1, 2, CAST(N'2025-07-08 09:00:00.000' AS DateTime), CAST(N'2025-07-08 18:00:00.000' AS DateTime))
INSERT [dbo].[LichLamViec] ([IdLichLamViec], [IdNhanVien], [ThoiGianBatDau], [ThoiGianKetThuc]) VALUES (2, 3, CAST(N'2025-07-08 13:00:00.000' AS DateTime), CAST(N'2025-07-08 21:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[LichLamViec] OFF
SET IDENTITY_INSERT [dbo].[NhanVien] ON 

INSERT [dbo].[NhanVien] ([IdNhanVien], [HoTen], [SoDienThoai], [ChucVu], [IdTaiKhoan]) VALUES (1, N'Trần Ngọc Anh', N'0987654321', N'Quản lý chi nhánh', 1)
INSERT [dbo].[NhanVien] ([IdNhanVien], [HoTen], [SoDienThoai], [ChucVu], [IdTaiKhoan]) VALUES (2, N'Nguyễn Ngọc Ánh', N'0912345678', N'Chuyên viên chăm sóc da', 2)
INSERT [dbo].[NhanVien] ([IdNhanVien], [HoTen], [SoDienThoai], [ChucVu], [IdTaiKhoan]) VALUES (3, N'Lê Thanh Huyền', N'0905112233', N'Chuyên viên trị liệu', 3)
SET IDENTITY_INSERT [dbo].[NhanVien] OFF
SET IDENTITY_INSERT [dbo].[SanPham] ON 

INSERT [dbo].[SanPham] ([IdSanPham], [TenSanPham], [MoTa], [Gia], [SoLuongTon], [HinhAnhURL], [IdDanhMuc]) VALUES (1, N'Serum cấp ẩm Hyaluronic Acid', N'Serum giúp cấp ẩm sâu, cho làn da căng bóng.', CAST(300000.00 AS Decimal(15, 2)), 49, N'/images/products/HA_dacf9f5f.png', 1)
INSERT [dbo].[SanPham] ([IdSanPham], [TenSanPham], [MoTa], [Gia], [SoLuongTon], [HinhAnhURL], [IdDanhMuc]) VALUES (2, N'Kem chống nắng SPF 50+ PA++++', N'Bảo vệ da tối ưu, không gây bết dính.', CAST(400000.00 AS Decimal(15, 2)), 100, N'/images/products/kcn_1df0d992.png', 3)
INSERT [dbo].[SanPham] ([IdSanPham], [TenSanPham], [MoTa], [Gia], [SoLuongTon], [HinhAnhURL], [IdDanhMuc]) VALUES (3, N'Sữa dưỡng thể Oliu', N'Dưỡng ẩm, làm mềm và mịn da toàn thân.', CAST(350000.00 AS Decimal(15, 2)), 74, N'/images/products/oliu_2d2a05b1.jpg', 2)
INSERT [dbo].[SanPham] ([IdSanPham], [TenSanPham], [MoTa], [Gia], [SoLuongTon], [HinhAnhURL], [IdDanhMuc]) VALUES (4, N' Tẩy Da Chết Paula’s Choice 2% BHA 30ml', N'Dung Dịch Tẩy Da Chết Paula’s Choice Skin Perfecting 2% BHA Liquid Exfoliant đến từ thương hiệu dược mỹ phẩm Paula''s Choice nổi tiếng của Mỹ là sản phẩm tẩy tế bào chết hóa học với nồng độ 2% BHA (Salicylic Acid) giúp làm sạch sâu lỗ chân lông', CAST(250000.00 AS Decimal(15, 2)), 18, N'/images/products/bha_31c2f758.png', 1)
INSERT [dbo].[SanPham] ([IdSanPham], [TenSanPham], [MoTa], [Gia], [SoLuongTon], [HinhAnhURL], [IdDanhMuc]) VALUES (6, N'Tẩy trang cho da dầu nhạy cảm La Roche-Posay Micellar Water 50m', N'ước tẩy trang giàu khoáng cho da nhạy cảm La Roche-Posay Micellar Water Ultra Sensitive Skin với công nghệ cải tiến Glyco Micellar mang lại hiệu quả làm sạch sâu vượt trội, giúp lấy đi lớp trang điểm, bã nhờn và các hạt bụi siêu nhỏ có trong khói xe và môi trường ô nhiễm nhưng vẫn an toàn cho làn da nhạy cảm.', CAST(600000.00 AS Decimal(15, 2)), 29, N'/images/products/TayTrang_03db5027.png', 1)
SET IDENTITY_INSERT [dbo].[SanPham] OFF
SET IDENTITY_INSERT [dbo].[TaiKhoan] ON 

INSERT [dbo].[TaiKhoan] ([IdTaiKhoan], [TenDangNhap], [MatKhauHash], [IdVaiTro], [NgayTao], [TrangThai]) VALUES (1, N'admin_spa', N'hashed_password_placeholder', 3, CAST(N'2025-07-12 22:21:44.203' AS DateTime), N'HoatDong')
INSERT [dbo].[TaiKhoan] ([IdTaiKhoan], [TenDangNhap], [MatKhauHash], [IdVaiTro], [NgayTao], [TrangThai]) VALUES (2, N'ngocanh.nv', N'hashed_password_placeholder', 2, CAST(N'2025-07-12 22:21:44.203' AS DateTime), N'HoatDong')
INSERT [dbo].[TaiKhoan] ([IdTaiKhoan], [TenDangNhap], [MatKhauHash], [IdVaiTro], [NgayTao], [TrangThai]) VALUES (3, N'thanhhuyen.nv', N'hashed_password_placeholder', 2, CAST(N'2025-07-12 22:21:44.203' AS DateTime), N'HoatDong')
INSERT [dbo].[TaiKhoan] ([IdTaiKhoan], [TenDangNhap], [MatKhauHash], [IdVaiTro], [NgayTao], [TrangThai]) VALUES (4, N'minhthu.kh', N'hashed_password_placeholder', 1, CAST(N'2025-07-12 22:21:44.203' AS DateTime), N'HoatDong')
INSERT [dbo].[TaiKhoan] ([IdTaiKhoan], [TenDangNhap], [MatKhauHash], [IdVaiTro], [NgayTao], [TrangThai]) VALUES (5, N'baotran.kh', N'hashed_password_placeholder', 1, CAST(N'2025-07-12 22:21:44.203' AS DateTime), N'HoatDong')
INSERT [dbo].[TaiKhoan] ([IdTaiKhoan], [TenDangNhap], [MatKhauHash], [IdVaiTro], [NgayTao], [TrangThai]) VALUES (6, N'quoctuan.kh', N'hashed_password_placeholder', 1, CAST(N'2025-07-12 22:21:44.203' AS DateTime), N'HoatDong')
INSERT [dbo].[TaiKhoan] ([IdTaiKhoan], [TenDangNhap], [MatKhauHash], [IdVaiTro], [NgayTao], [TrangThai]) VALUES (7, N'Dinh', N'f5HopLZIsBJbFdxaO2Rm+fSQbZLHK+qb1r6SyFO+vaI=', 1, CAST(N'2025-07-13 15:22:53.327' AS DateTime), N'HoatDong')
INSERT [dbo].[TaiKhoan] ([IdTaiKhoan], [TenDangNhap], [MatKhauHash], [IdVaiTro], [NgayTao], [TrangThai]) VALUES (8, N'Test', N'f5HopLZIsBJbFdxaO2Rm+fSQbZLHK+qb1r6SyFO+vaI=', 1, CAST(N'2025-07-13 23:20:30.130' AS DateTime), N'HoatDong')
SET IDENTITY_INSERT [dbo].[TaiKhoan] OFF
SET IDENTITY_INSERT [dbo].[ThanhToan] ON 

INSERT [dbo].[ThanhToan] ([IdThanhToan], [IdDonHang], [IdLichHen], [SoTien], [PhuongThucThanhToan], [ThoiGianThanhToan], [TrangThai], [MaGiaoDich]) VALUES (1, 1, NULL, CAST(1030000.00 AS Decimal(15, 2)), N'Thẻ Tín Dụng', CAST(N'2025-07-12 22:21:44.247' AS DateTime), N'DaThanhToan', N'VNPAY_XYZ123')
INSERT [dbo].[ThanhToan] ([IdThanhToan], [IdDonHang], [IdLichHen], [SoTien], [PhuongThucThanhToan], [ThoiGianThanhToan], [TrangThai], [MaGiaoDich]) VALUES (2, NULL, 2, CAST(200000.00 AS Decimal(15, 2)), N'Chuyển Khoản', CAST(N'2025-07-12 22:21:44.247' AS DateTime), N'DaThanhToan', NULL)
SET IDENTITY_INSERT [dbo].[ThanhToan] OFF
SET IDENTITY_INSERT [dbo].[TinNhanHoTro] ON 

INSERT [dbo].[TinNhanHoTro] ([IdTinNhan], [IdNguoiGui], [IdNguoiNhan], [NoiDung], [ThoiGianGui], [DaXem]) VALUES (1, 5, 3, N'Chào bạn, mình muốn hỏi về liệu trình massage đá nóng.', CAST(N'2025-07-12 22:21:44.250' AS DateTime), 0)
INSERT [dbo].[TinNhanHoTro] ([IdTinNhan], [IdNguoiGui], [IdNguoiNhan], [NoiDung], [ThoiGianGui], [DaXem]) VALUES (2, 3, 5, N'Dạ chào bạn Bảo Trân, liệu trình này kéo dài 90 phút và rất tốt cho việc thư giãn cơ bắp ạ. Bạn có muốn đặt lịch không ạ?', CAST(N'2025-07-12 22:21:44.250' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[TinNhanHoTro] OFF
SET IDENTITY_INSERT [dbo].[VaiTro] ON 

INSERT [dbo].[VaiTro] ([IdVaiTro], [TenVaiTro]) VALUES (1, N'KhachHang')
INSERT [dbo].[VaiTro] ([IdVaiTro], [TenVaiTro]) VALUES (2, N'NhanVien')
INSERT [dbo].[VaiTro] ([IdVaiTro], [TenVaiTro]) VALUES (3, N'QuanLy')
SET IDENTITY_INSERT [dbo].[VaiTro] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__DanhMucD__650CAE4ECC61096F]    Script Date: 20/07/2025 23:52:08 ******/
ALTER TABLE [dbo].[DanhMucDichVu] ADD UNIQUE NONCLUSTERED 
(
	[TenDanhMuc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__DanhMucS__650CAE4E942AF072]    Script Date: 20/07/2025 23:52:08 ******/
ALTER TABLE [dbo].[DanhMucSanPham] ADD UNIQUE NONCLUSTERED 
(
	[TenDanhMuc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__KhachHan__0389B7BDD693E2C4]    Script Date: 20/07/2025 23:52:08 ******/
ALTER TABLE [dbo].[KhachHang] ADD UNIQUE NONCLUSTERED 
(
	[SoDienThoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UQ__KhachHan__9A53D3DC03D9F5E7]    Script Date: 20/07/2025 23:52:08 ******/
ALTER TABLE [dbo].[KhachHang] ADD UNIQUE NONCLUSTERED 
(
	[IdTaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__KhachHan__A9D1053483AB95F7]    Script Date: 20/07/2025 23:52:08 ******/
ALTER TABLE [dbo].[KhachHang] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__KhuyenMa__6F56B3BC91DF21B0]    Script Date: 20/07/2025 23:52:08 ******/
ALTER TABLE [dbo].[KhuyenMai] ADD UNIQUE NONCLUSTERED 
(
	[MaKhuyenMai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__NhanVien__0389B7BDC294C9D4]    Script Date: 20/07/2025 23:52:08 ******/
ALTER TABLE [dbo].[NhanVien] ADD UNIQUE NONCLUSTERED 
(
	[SoDienThoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UQ__NhanVien__9A53D3DC995BBDF9]    Script Date: 20/07/2025 23:52:08 ******/
ALTER TABLE [dbo].[NhanVien] ADD UNIQUE NONCLUSTERED 
(
	[IdTaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__TaiKhoan__55F68FC0BD634A09]    Script Date: 20/07/2025 23:52:08 ******/
ALTER TABLE [dbo].[TaiKhoan] ADD UNIQUE NONCLUSTERED 
(
	[TenDangNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__VaiTro__1DA558141C9C7F6A]    Script Date: 20/07/2025 23:52:08 ******/
ALTER TABLE [dbo].[VaiTro] ADD UNIQUE NONCLUSTERED 
(
	[TenVaiTro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD FOREIGN KEY([IdDonHang])
REFERENCES [dbo].[DonHang] ([IdDonHang])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD FOREIGN KEY([IdSanPham])
REFERENCES [dbo].[SanPham] ([IdSanPham])
GO
ALTER TABLE [dbo].[DanhGia]  WITH CHECK ADD FOREIGN KEY([IdDichVu])
REFERENCES [dbo].[DichVu] ([IdDichVu])
GO
ALTER TABLE [dbo].[DanhGia]  WITH CHECK ADD FOREIGN KEY([IdKhachHang])
REFERENCES [dbo].[KhachHang] ([IdKhachHang])
GO
ALTER TABLE [dbo].[DanhGia]  WITH CHECK ADD FOREIGN KEY([IdSanPham])
REFERENCES [dbo].[SanPham] ([IdSanPham])
GO
ALTER TABLE [dbo].[DichVu]  WITH CHECK ADD FOREIGN KEY([IdDanhMuc])
REFERENCES [dbo].[DanhMucDichVu] ([IdDanhMuc])
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD FOREIGN KEY([IdKhachHang])
REFERENCES [dbo].[KhachHang] ([IdKhachHang])
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD FOREIGN KEY([IdKhuyenMai])
REFERENCES [dbo].[KhuyenMai] ([IdKhuyenMai])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD FOREIGN KEY([IdNhanVien])
REFERENCES [dbo].[NhanVien] ([IdNhanVien])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[KhachHang]  WITH CHECK ADD FOREIGN KEY([IdTaiKhoan])
REFERENCES [dbo].[TaiKhoan] ([IdTaiKhoan])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[LichHen]  WITH CHECK ADD FOREIGN KEY([IdDichVu])
REFERENCES [dbo].[DichVu] ([IdDichVu])
GO
ALTER TABLE [dbo].[LichHen]  WITH CHECK ADD FOREIGN KEY([IdKhachHang])
REFERENCES [dbo].[KhachHang] ([IdKhachHang])
GO
ALTER TABLE [dbo].[LichHen]  WITH CHECK ADD FOREIGN KEY([IdNhanVien])
REFERENCES [dbo].[NhanVien] ([IdNhanVien])
GO
ALTER TABLE [dbo].[LichLamViec]  WITH CHECK ADD FOREIGN KEY([IdNhanVien])
REFERENCES [dbo].[NhanVien] ([IdNhanVien])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD FOREIGN KEY([IdTaiKhoan])
REFERENCES [dbo].[TaiKhoan] ([IdTaiKhoan])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD FOREIGN KEY([IdDanhMuc])
REFERENCES [dbo].[DanhMucSanPham] ([IdDanhMuc])
GO
ALTER TABLE [dbo].[TaiKhoan]  WITH CHECK ADD FOREIGN KEY([IdVaiTro])
REFERENCES [dbo].[VaiTro] ([IdVaiTro])
GO
ALTER TABLE [dbo].[ThanhToan]  WITH CHECK ADD FOREIGN KEY([IdDonHang])
REFERENCES [dbo].[DonHang] ([IdDonHang])
GO
ALTER TABLE [dbo].[ThanhToan]  WITH CHECK ADD FOREIGN KEY([IdLichHen])
REFERENCES [dbo].[LichHen] ([IdLichHen])
GO
ALTER TABLE [dbo].[TinNhanHoTro]  WITH CHECK ADD FOREIGN KEY([IdNguoiGui])
REFERENCES [dbo].[TaiKhoan] ([IdTaiKhoan])
GO
ALTER TABLE [dbo].[TinNhanHoTro]  WITH CHECK ADD FOREIGN KEY([IdNguoiNhan])
REFERENCES [dbo].[TaiKhoan] ([IdTaiKhoan])
GO
ALTER TABLE [dbo].[DanhGia]  WITH CHECK ADD CHECK  (([SoSao]>=(1) AND [SoSao]<=(5)))
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD CHECK  (([TrangThai]=N'DaHuy' OR [TrangThai]=N'DaGiao' OR [TrangThai]=N'DangGiao' OR [TrangThai]=N'DangXuLy' OR [TrangThai]=N'ChoThanhToan'))
GO
ALTER TABLE [dbo].[KhuyenMai]  WITH CHECK ADD CHECK  (([ApDungCho]=N'TatCa' OR [ApDungCho]=N'DichVu' OR [ApDungCho]=N'SanPham'))
GO
ALTER TABLE [dbo].[LichHen]  WITH CHECK ADD CHECK  (([TrangThai]=N'ChoThanhToan' OR [TrangThai]=N'KhongDen' OR [TrangThai]=N'DaHuy' OR [TrangThai]=N'DaHoanThanh' OR [TrangThai]=N'DaDat'))
GO
ALTER TABLE [dbo].[TaiKhoan]  WITH CHECK ADD CHECK  (([TrangThai]=N'Khoa' OR [TrangThai]=N'HoatDong'))
GO
ALTER TABLE [dbo].[ThanhToan]  WITH CHECK ADD CHECK  (([TrangThai]=N'ThatBai' OR [TrangThai]=N'DaThanhToan' OR [TrangThai]=N'ChuaThanhToan'))
GO
