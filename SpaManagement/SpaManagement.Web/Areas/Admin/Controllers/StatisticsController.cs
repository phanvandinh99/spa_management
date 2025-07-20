using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SpaManagement.Web.Models.EF;
using SpaManagement.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace SpaManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StatisticsController : Controller
    {
        private readonly SpaDbContext _context;
        public StatisticsController(SpaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // API: Thống kê đơn hàng
        [HttpGet]
        public async Task<IActionResult> GetOrderStatistics(string period = "month", DateTime? from = null, DateTime? to = null)
        {
            var query = _context.DonHang.AsQueryable();
            if (period == "range" && from.HasValue && to.HasValue)
            {
                query = query.Where(d => d.NgayDatHang.Date >= from.Value.Date && d.NgayDatHang.Date <= to.Value.Date);
            }
            var result = new List<object>();
            if (period == "day")
            {
                result = await query.GroupBy(d => d.NgayDatHang.Date)
                    .Select(g => new { Label = g.Key.ToString("dd/MM/yyyy"), Count = g.Count(), Revenue = g.Sum(x => x.TongTien) })
                    .OrderBy(x => x.Label).ToListAsync<object>();
            }
            else if (period == "month")
            {
                result = await query.GroupBy(d => new { d.NgayDatHang.Year, d.NgayDatHang.Month })
                    .Select(g => new { Year = g.Key.Year, Month = g.Key.Month, Label = $"{g.Key.Month}/{g.Key.Year}", Count = g.Count(), Revenue = g.Sum(x => x.TongTien) })
                    .OrderBy(x => x.Year).ThenBy(x => x.Month).ToListAsync<object>();
            }
            else if (period == "quarter")
            {
                result = await query.GroupBy(d => new { d.NgayDatHang.Year, Quarter = (d.NgayDatHang.Month - 1) / 3 + 1 })
                    .Select(g => new { Year = g.Key.Year, Quarter = g.Key.Quarter, Label = $"Q{g.Key.Quarter}/{g.Key.Year}", Count = g.Count(), Revenue = g.Sum(x => x.TongTien) })
                    .OrderBy(x => x.Year).ThenBy(x => x.Quarter).ToListAsync<object>();
            }
            else if (period == "year")
            {
                result = await query.GroupBy(d => d.NgayDatHang.Year)
                    .Select(g => new { Year = g.Key, Label = g.Key.ToString(), Count = g.Count(), Revenue = g.Sum(x => x.TongTien) })
                    .OrderBy(x => x.Year).ToListAsync<object>();
            }
            else if (period == "range")
            {
                result = await query.GroupBy(d => d.NgayDatHang.Date)
                    .Select(g => new { Label = g.Key.ToString("dd/MM/yyyy"), Count = g.Count(), Revenue = g.Sum(x => x.TongTien) })
                    .OrderBy(x => x.Label).ToListAsync<object>();
            }
            return Json(result);
        }

        // API: Thống kê sản phẩm bán ra
        [HttpGet]
        public async Task<IActionResult> GetProductStatistics(string period = "month", DateTime? from = null, DateTime? to = null)
        {
            var query = _context.ChiTietDonHang.Include(c => c.SanPham).Include(c => c.DonHang).AsQueryable();
            if (period == "range" && from.HasValue && to.HasValue)
            {
                query = query.Where(c => c.DonHang.NgayDatHang.Date >= from.Value.Date && c.DonHang.NgayDatHang.Date <= to.Value.Date);
            }
            var result = await query.GroupBy(c => c.SanPham.TenSanPham)
                .Select(g => new { Label = g.Key, Quantity = g.Sum(x => x.SoLuong), Revenue = g.Sum(x => x.SoLuong * x.DonGia) })
                .OrderByDescending(x => x.Quantity).ToListAsync();
            return Json(result);
        }

        // API: Thống kê dịch vụ (theo lịch hẹn)
        [HttpGet]
        public async Task<IActionResult> GetServiceStatistics(string period = "month", DateTime? from = null, DateTime? to = null)
        {
            var query = _context.LichHen.Include(l => l.DichVu).AsQueryable();
            if (period == "range" && from.HasValue && to.HasValue)
            {
                query = query.Where(l => l.ThoiGianBatDau.Date >= from.Value.Date && l.ThoiGianBatDau.Date <= to.Value.Date);
            }
            // Thống kê theo từng mốc thời gian nếu cần, mặc định group theo dịch vụ
            var result = await query.GroupBy(l => l.DichVu.TenDichVu)
                .Select(g => new { Label = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count).ToListAsync();
            return Json(result);
        }

        // API: Thống kê doanh thu tổng hợp (có thể dùng lại GetOrderStatistics)
        [HttpGet]
        public async Task<IActionResult> GetRevenueStatistics(string period = "month", DateTime? from = null, DateTime? to = null)
        {
            var query = _context.DonHang.AsQueryable();
            if (period == "range" && from.HasValue && to.HasValue)
            {
                query = query.Where(d => d.NgayDatHang.Date >= from.Value.Date && d.NgayDatHang.Date <= to.Value.Date);
            }
            var result = new List<object>();
            if (period == "day")
            {
                result = await query.GroupBy(d => d.NgayDatHang.Date)
                    .Select(g => new { Label = g.Key.ToString("dd/MM/yyyy"), Revenue = g.Sum(x => x.TongTien) })
                    .OrderBy(x => x.Label).ToListAsync<object>();
            }
            else if (period == "month")
            {
                result = await query.GroupBy(d => new { d.NgayDatHang.Year, d.NgayDatHang.Month })
                    .Select(g => new { Year = g.Key.Year, Month = g.Key.Month, Label = $"{g.Key.Month}/{g.Key.Year}", Revenue = g.Sum(x => x.TongTien) })
                    .OrderBy(x => x.Year).ThenBy(x => x.Month).ToListAsync<object>();
            }
            else if (period == "quarter")
            {
                result = await query.GroupBy(d => new { d.NgayDatHang.Year, Quarter = (d.NgayDatHang.Month - 1) / 3 + 1 })
                    .Select(g => new { Year = g.Key.Year, Quarter = g.Key.Quarter, Label = $"Q{g.Key.Quarter}/{g.Key.Year}", Revenue = g.Sum(x => x.TongTien) })
                    .OrderBy(x => x.Year).ThenBy(x => x.Quarter).ToListAsync<object>();
            }
            else if (period == "year")
            {
                result = await query.GroupBy(d => d.NgayDatHang.Year)
                    .Select(g => new { Year = g.Key, Label = g.Key.ToString(), Revenue = g.Sum(x => x.TongTien) })
                    .OrderBy(x => x.Year).ToListAsync<object>();
            }
            else if (period == "range")
            {
                result = await query.GroupBy(d => d.NgayDatHang.Date)
                    .Select(g => new { Label = g.Key.ToString("dd/MM/yyyy"), Revenue = g.Sum(x => x.TongTien) })
                    .OrderBy(x => x.Label).ToListAsync<object>();
            }
            return Json(result);
        }

        // API: Thống kê tổng hợp (đơn hàng, dịch vụ, doanh thu) theo khoảng ngày
        [HttpGet]
        public async Task<IActionResult> GetAllStatistics(DateTime? from = null, DateTime? to = null)
        {
            try
            {
                // Đơn hàng
                var ordersQuery = _context.DonHang.AsQueryable();
                if (from.HasValue) ordersQuery = ordersQuery.Where(d => d.NgayDatHang.Date >= from.Value.Date);
                if (to.HasValue) ordersQuery = ordersQuery.Where(d => d.NgayDatHang.Date <= to.Value.Date);
                var orders = await ordersQuery.ToListAsync();
                var orderStats = new {
                    TotalOrders = orders.Count,
                    TotalRevenue = orders.Sum(x => x.TongTien),
                    ByStatus = orders.GroupBy(x => x.TrangThai).Select(g => new { Status = g.Key, Count = g.Count() }).ToList()
                };

                // Dịch vụ (theo lịch hẹn)
                var servicesQuery = _context.LichHen.Include(l => l.DichVu).AsQueryable();
                if (from.HasValue) servicesQuery = servicesQuery.Where(l => l.ThoiGianBatDau.Date >= from.Value.Date);
                if (to.HasValue) servicesQuery = servicesQuery.Where(l => l.ThoiGianBatDau.Date <= to.Value.Date);
                var services = await servicesQuery.ToListAsync();
                var serviceStats = services.GroupBy(l => l.DichVu.TenDichVu)
                    .Select(g => new { Service = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count).ToList();

                // Doanh thu theo ngày
                var revenueByDay = orders.GroupBy(x => x.NgayDatHang.Date)
                    .Select(g => new { Date = g.Key.ToString("dd/MM/yyyy"), Revenue = g.Sum(x => x.TongTien) })
                    .OrderBy(x => x.Date).ToList();

                return Json(new {
                    Orders = orderStats,
                    Services = serviceStats,
                    RevenueByDay = revenueByDay
                });
            }
            catch (Exception ex)
            {
                // Log lỗi ra console server
                System.Diagnostics.Debug.WriteLine($"[StatisticsController] Lỗi GetAllStatistics: {ex.Message}\n{ex.StackTrace}");
                return Json(new { error = true, message = "Lỗi khi lấy dữ liệu thống kê: " + ex.Message });
            }
        }
    }
} 