using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SpaManagement.Web.Models.EF;
using SpaManagement.Web.Models;
using System.Security.Claims;

namespace SpaManagement.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class OrdersController : Controller
    {
        private readonly SpaDbContext _context;

        public OrdersController(SpaDbContext context)
        {
            _context = context;
        }

        // Danh sách đơn hàng
        public async Task<IActionResult> Index()
        {
            var userName = User.Identity.Name;
            var khachHang = await _context.KhachHang.Include(kh => kh.TaiKhoan)
                .FirstOrDefaultAsync(kh => kh.TaiKhoan != null && kh.TaiKhoan.TenDangNhap == userName);
            if (khachHang == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Customer" });
            }

            var donHangs = await _context.DonHang
                .Include(dh => dh.ChiTietDonHangs)
                    .ThenInclude(ct => ct.SanPham)
                .Where(dh => dh.IdKhachHang == khachHang.IdKhachHang)
                .OrderByDescending(dh => dh.NgayDatHang)
                .ToListAsync();

            return View(donHangs);
        }

        // Chi tiết đơn hàng
        public async Task<IActionResult> OrderDetails(int id)
        {
            var userName = User.Identity.Name;
            var khachHang = await _context.KhachHang.Include(kh => kh.TaiKhoan)
                .FirstOrDefaultAsync(kh => kh.TaiKhoan != null && kh.TaiKhoan.TenDangNhap == userName);
            if (khachHang == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Customer" });
            }

            var donHang = await _context.DonHang
                .Include(dh => dh.ChiTietDonHangs)
                    .ThenInclude(ct => ct.SanPham)
                .Include(dh => dh.KhuyenMai)
                .FirstOrDefaultAsync(dh => dh.IdDonHang == id && dh.IdKhachHang == khachHang.IdKhachHang);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // Hủy đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userName = User.Identity.Name;
            var khachHang = await _context.KhachHang.Include(kh => kh.TaiKhoan)
                .FirstOrDefaultAsync(kh => kh.TaiKhoan != null && kh.TaiKhoan.TenDangNhap == userName);
            if (khachHang == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Customer" });
            }

            var donHang = await _context.DonHang
                .Include(dh => dh.ChiTietDonHangs)
                .FirstOrDefaultAsync(dh => dh.IdDonHang == id && dh.IdKhachHang == khachHang.IdKhachHang);

            if (donHang == null)
            {
                TempData["Error"] = "Đơn hàng không tồn tại";
                return RedirectToAction("Index");
            }

            if (donHang.TrangThai != "ChoThanhToan" && donHang.TrangThai != "DangXuLy")
            {
                TempData["Error"] = "Không thể hủy đơn hàng ở trạng thái này";
                return RedirectToAction("OrderDetails", new { id = donHang.IdDonHang });
            }

            // Hoàn trả số lượng tồn kho
            foreach (var chiTiet in donHang.ChiTietDonHangs)
            {
                var sanPham = await _context.SanPham.FindAsync(chiTiet.IdSanPham);
                if (sanPham != null)
                {
                    sanPham.SoLuongTon += chiTiet.SoLuong;
                }
            }

            donHang.TrangThai = "DaHuy";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã hủy đơn hàng thành công";
            return RedirectToAction("OrderDetails", new { id = donHang.IdDonHang });
        }
    }
} 