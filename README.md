# Hệ Thống Quản Lý Spa

Hệ thống quản lý spa được xây dựng bằng ASP.NET Core và SQL Server với database `QuanLySpa`.

## 🚀 Cài đặt và Chạy

### 1. Yêu cầu hệ thống

- .NET 8.0 SDK
- SQL Server (LocalDB, Express, hoặc Developer Edition)
- Visual Studio 2022 hoặc VS Code

### 2. Thiết lập Database

#### Bước 1: Tạo Database

1. Mở **SQL Server Management Studio (SSMS)**
2. Kết nối đến SQL Server của bạn
3. Mở file `Database/Database_QuanLySpa.sql`
4. Chạy toàn bộ script để tạo database và dữ liệu mẫu

#### Bước 2: Kiểm tra kết nối

```sql
-- Kiểm tra database đã được tạo
SELECT name FROM sys.databases WHERE name = 'QuanLySpa';

-- Kiểm tra các bảng
USE QuanLySpa;
SELECT table_name FROM information_schema.tables WHERE table_type = 'BASE TABLE';
```

### 3. Cấu hình Connection String

File `SpaManagement/SpaManagement.Web/appsettings.json` đã được cấu hình:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=QuanLySpa;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

**Lưu ý quan trọng:**

- Thay đổi `Server=localhost` nếu SQL Server của bạn chạy trên server khác
- Nếu dùng SQL Authentication: thay `Trusted_Connection=true` bằng `User Id=username;Password=password`

### 4. Chạy ứng dụng

```bash
# Di chuyển vào thư mục project
cd SpaManagement/SpaManagement.Web

# Restore packages
dotnet restore

# Build project
dotnet build

# Chạy ứng dụng
dotnet run
```

Ứng dụng sẽ chạy tại: `https://localhost:7000` hoặc `http://localhost:5000`

## 📊 Cấu trúc Database

### Bảng chính:

- **VaiTro**: Quản lý vai trò (Khách hàng, Nhân viên, Quản lý)
- **TaiKhoan**: Tài khoản đăng nhập
- **KhachHang**: Thông tin khách hàng
- **NhanVien**: Thông tin nhân viên
- **DanhMucDichVu**: Danh mục dịch vụ
- **DichVu**: Các dịch vụ spa
- **LichHen**: Lịch hẹn của khách hàng
- **LichLamViec**: Lịch làm việc của nhân viên
- **DanhMucSanPham**: Danh mục sản phẩm
- **SanPham**: Sản phẩm bán
- **KhuyenMai**: Chương trình khuyến mãi
- **DonHang**: Đơn hàng mua sản phẩm
- **ChiTietDonHang**: Chi tiết đơn hàng
- **ThanhToan**: Quản lý thanh toán
- **DanhGia**: Đánh giá của khách hàng
- **TinNhanHoTro**: Chat hỗ trợ

### Dữ liệu mẫu:

- 3 vai trò: Khách hàng, Nhân viên, Quản lý
- 6 tài khoản với dữ liệu mẫu
- 3 nhân viên và 3 khách hàng
- 3 danh mục dịch vụ và 3 dịch vụ mẫu
- 3 danh mục sản phẩm và 3 sản phẩm mẫu
- Lịch hẹn và đơn hàng mẫu

## 🔧 Tính năng đã hoàn thành

- ✅ Kết nối SQL Server với Entity Framework Core
- ✅ Models với Data Annotations và validation
- ✅ DbContext với tất cả các DbSet
- ✅ Database First approach với database có sẵn
- ✅ Sample data đầy đủ
- ✅ Relationship mapping chính xác

## 📝 Các bước tiếp theo

1. **Tạo Controllers** cho CRUD operations
2. **Tạo Views** cho giao diện người dùng
3. **Thêm Authentication** và Authorization
4. **Implement Business Logic**
5. **Thêm Validation** và Error Handling
6. **Tạo API Endpoints** cho mobile app

## 🛠️ Troubleshooting

### Lỗi kết nối database

```bash
# Kiểm tra SQL Server đã chạy chưa
sqlcmd -S localhost -E -Q "SELECT @@VERSION"

# Kiểm tra database tồn tại
sqlcmd -S localhost -E -Q "SELECT name FROM sys.databases WHERE name = 'QuanLySpa'"
```

### Lỗi Entity Framework

```bash
# Tạo migration (nếu cần)
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### Lỗi build project

```bash
# Clean và rebuild
dotnet clean
dotnet restore
dotnet build
```

### Lỗi "The type or namespace name 'X' could not be found"

Nếu gặp lỗi này, hãy kiểm tra:

1. **Tất cả model classes đã được tạo:**

   - VaiTro.cs, TaiKhoan.cs, KhachHang.cs, NhanVien.cs
   - DanhMucDichVu.cs, DichVu.cs, LichHen.cs, LichLamViec.cs
   - DanhMucSanPham.cs, SanPham.cs, KhuyenMai.cs
   - DonHang.cs, ChiTietDonHang.cs, ThanhToan.cs
   - DanhGia.cs, TinNhanHoTro.cs

2. **SpaDbContext.cs đã được tạo đúng:**

   ```csharp
   public DbSet<VaiTro> VaiTro { get; set; }
   public DbSet<TaiKhoan> TaiKhoan { get; set; }
   // ... tất cả các DbSet khác
   ```

3. **Namespace đúng:**

   - Tất cả models phải có namespace `SpaManagement.Web.Models`
   - SpaDbContext phải có namespace `SpaManagement.Web.Models.EF`

4. **Build lại project:**
   ```bash
   dotnet clean
   dotnet restore
   dotnet build
   ```

### Lỗi "Missing using directive"

Thêm các using statements cần thiết vào đầu file:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
```

### Lỗi Relationship trong Entity Framework

Nếu gặp lỗi relationship, hãy kiểm tra:

1. **TinNhanHoTro đã được comment out** - hiện tại chưa sử dụng
2. **Navigation properties** trong TaiKhoan.cs đã được comment
3. **DbContext configuration** đã được comment tương ứng

Để sử dụng TinNhanHoTro sau này:

1. Uncomment các navigation properties trong TaiKhoan.cs
2. Uncomment DbSet trong SpaDbContext.cs
3. Uncomment và cấu hình relationship trong OnModelCreating

## 📞 Hỗ trợ

Nếu gặp vấn đề, hãy kiểm tra:

1. SQL Server đã chạy chưa
2. Connection string có đúng không
3. Database đã được tạo chưa
4. Entity Framework packages đã được install chưa
5. Tất cả model classes đã được tạo đầy đủ
