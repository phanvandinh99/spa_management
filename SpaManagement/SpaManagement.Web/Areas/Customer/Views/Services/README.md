# Trang Dịch vụ - Spa Management

## Tổng quan

Trang dịch vụ cung cấp giao diện cho khách hàng xem danh sách các dịch vụ spa và chi tiết từng dịch vụ.

## Các tính năng chính

### 1. Danh sách dịch vụ (Index)

- **Hiển thị tất cả dịch vụ**: Hiển thị danh sách tất cả dịch vụ có trong hệ thống
- **Bộ lọc theo danh mục**: Cho phép lọc dịch vụ theo từng danh mục cụ thể
- **Thông tin dịch vụ**: Hiển thị tên, mô tả, giá, thời lượng, danh mục
- **Đánh giá**: Hiển thị số sao trung bình và số lượng đánh giá
- **Nút chi tiết**: Dẫn đến trang chi tiết dịch vụ

### 2. Chi tiết dịch vụ (Details)

- **Thông tin đầy đủ**: Tên, mô tả, giá, thời lượng, danh mục
- **Đánh giá tổng hợp**: Hiển thị điểm trung bình và số lượng đánh giá
- **Danh sách đánh giá**: Hiển thị tất cả đánh giá từ khách hàng
- **Nút đặt lịch**: Dẫn đến trang đặt lịch hẹn
- **Breadcrumb**: Điều hướng dễ dàng

## Cấu trúc Controller

### ServicesController

```csharp
public class ServicesController : Controller
{
    private readonly SpaDbContext _context;

    // Constructor với dependency injection
    public ServicesController(SpaDbContext context)

    // Action danh sách dịch vụ
    public async Task<IActionResult> Index()

    // Action chi tiết dịch vụ
    public async Task<IActionResult> Details(int id)

    // Action lọc theo danh mục
    public async Task<IActionResult> Category(int? categoryId)
}
```

## Cấu trúc Model

### DichVu

```csharp
public class DichVu
{
    public int IdDichVu { get; set; }
    public string TenDichVu { get; set; }
    public string? MoTa { get; set; }
    public decimal Gia { get; set; }
    public int? ThoiLuongPhut { get; set; }
    public int? IdDanhMuc { get; set; }

    // Navigation properties
    public virtual DanhMucDichVu? DanhMucDichVu { get; set; }
    public virtual ICollection<DanhGia> DanhGias { get; set; }
}
```

## Routes

### Danh sách dịch vụ

- **URL**: `/Customer/Services/Index`
- **Method**: GET
- **Parameters**: Không

### Chi tiết dịch vụ

- **URL**: `/Customer/Services/Details/{id}`
- **Method**: GET
- **Parameters**:
  - `id` (int): ID của dịch vụ

### Lọc theo danh mục

- **URL**: `/Customer/Services/Category/{categoryId}`
- **Method**: GET
- **Parameters**:
  - `categoryId` (int?): ID của danh mục (optional)

## Giao diện

### Danh sách dịch vụ

- Layout responsive với Bootstrap
- Cards hiển thị thông tin dịch vụ
- Bộ lọc danh mục dạng button group
- Hiệu ứng hover cho cards
- Hiển thị giá với gradient background
- Hiển thị đánh giá bằng sao

### Chi tiết dịch vụ

- Layout 2 cột (8-4)
- Breadcrumb navigation
- Thông tin chi tiết dịch vụ
- Sidebar với đánh giá và thông tin nhanh
- Phần đánh giá từ khách hàng
- Nút đặt lịch và quay lại

## CSS Classes

### Danh sách dịch vụ

- `.service-card`: Card chứa thông tin dịch vụ
- `.price-tag`: Hiển thị giá với gradient
- `.stars`: Hiển thị đánh giá sao
- `.service-details`: Thông tin chi tiết dịch vụ
- `.service-rating`: Phần đánh giá

### Chi tiết dịch vụ

- `.price-display`: Hiển thị giá nổi bật
- `.description-content`: Nội dung mô tả
- `.feature-item`: Các đặc điểm dịch vụ
- `.rating-summary`: Tổng hợp đánh giá
- `.review-item`: Từng đánh giá khách hàng

## Dữ liệu mẫu

File SQL đã có sẵn dữ liệu mẫu cho:

- Danh mục dịch vụ: Massage trị liệu, Chăm sóc da chuyên sâu, Triệt lông công nghệ cao
- Dịch vụ: Massage đá nóng, Chăm sóc da mặt Aqua Peel, Triệt lông vùng nách
- Đánh giá từ khách hàng
- Thông tin giá và thời lượng

## Lưu ý

1. **Font Awesome**: Đã được include trong layout để hiển thị icons
2. **Responsive**: Giao diện responsive trên mọi thiết bị
3. **Performance**: Sử dụng async/await cho database queries
4. **Error Handling**: Có xử lý trường hợp không tìm thấy dịch vụ
5. **Navigation**: Breadcrumb và nút quay lại để điều hướng dễ dàng
