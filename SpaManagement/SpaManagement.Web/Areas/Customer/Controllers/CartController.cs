using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SpaManagement.Web.Models.EF;
using SpaManagement.Web.Models;
using System.Security.Claims;
using System.Text.Json;

namespace SpaManagement.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly SpaDbContext _context;

        public CartController(SpaDbContext context)
        {
            _context = context;
        }

        // Model cho giỏ hàng
        public class CartItem
        {
            public int IdSanPham { get; set; }
            public string TenSanPham { get; set; } = string.Empty;
            public decimal Gia { get; set; }
            public int SoLuong { get; set; }
            public string? HinhAnhURL { get; set; }
        }

        // Lấy giỏ hàng từ session
        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return cartJson != null ? JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>() : new List<CartItem>();
        }

        // Lưu giỏ hàng vào session
        private void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }

        // Xem giỏ hàng
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // Lấy số lượng giỏ hàng
        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = GetCart();
            return Json(cart.Count);
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var sanPham = await _context.SanPham.FindAsync(productId);
            if (sanPham == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại" });
            }

            if (sanPham.SoLuongTon < quantity)
            {
                return Json(new { success = false, message = "Số lượng tồn kho không đủ" });
            }

            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(item => item.IdSanPham == productId);

            if (existingItem != null)
            {
                existingItem.SoLuong += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    IdSanPham = sanPham.IdSanPham,
                    TenSanPham = sanPham.TenSanPham,
                    Gia = sanPham.Gia,
                    SoLuong = quantity,
                    HinhAnhURL = sanPham.HinhAnhURL
                });
            }

            SaveCart(cart);
            return Json(new { success = true, message = "Đã thêm vào giỏ hàng", cartCount = cart.Count });
        }

        // Cập nhật số lượng
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return Json(new { success = false, message = "Số lượng phải lớn hơn 0" });
            }

            var sanPham = await _context.SanPham.FindAsync(productId);
            if (sanPham == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại" });
            }

            if (sanPham.SoLuongTon < quantity)
            {
                return Json(new { success = false, message = "Số lượng tồn kho không đủ" });
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.IdSanPham == productId);
            if (item != null)
            {
                item.SoLuong = quantity;
                SaveCart(cart);
                return Json(new { success = true, total = cart.Sum(i => i.Gia * i.SoLuong) });
            }

            return Json(new { success = false, message = "Sản phẩm không có trong giỏ hàng" });
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.IdSanPham == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
                return Json(new { success = true, cartCount = cart.Count, total = cart.Sum(i => i.Gia * i.SoLuong) });
            }

            return Json(new { success = false, message = "Sản phẩm không có trong giỏ hàng" });
        }

        // Thanh toán
        [Authorize(Roles = "KhachHang")]
        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["Error"] = "Giỏ hàng trống";
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        [Authorize(Roles = "KhachHang")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(string diaChiGiaoHang, string? ghiChu)
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["Error"] = "Giỏ hàng trống";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(diaChiGiaoHang))
            {
                ModelState.AddModelError("", "Địa chỉ giao hàng là bắt buộc");
                return View(cart);
            }

            // Lấy thông tin khách hàng
            var userName = User.Identity.Name;
            var khachHang = await _context.KhachHang.Include(kh => kh.TaiKhoan)
                .FirstOrDefaultAsync(kh => kh.TaiKhoan != null && kh.TaiKhoan.TenDangNhap == userName);
            if (khachHang == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Customer" });
            }

            // Kiểm tra tồn kho
            foreach (var item in cart)
            {
                var sanPham = await _context.SanPham.FindAsync(item.IdSanPham);
                if (sanPham == null || sanPham.SoLuongTon < item.SoLuong)
                {
                    TempData["Error"] = $"Sản phẩm {item.TenSanPham} không đủ số lượng tồn kho";
                    return RedirectToAction("Index");
                }
            }

            // Tạo đơn hàng
            var donHang = new DonHang
            {
                IdKhachHang = khachHang.IdKhachHang,
                NgayDatHang = DateTime.Now,
                TongTien = cart.Sum(i => i.Gia * i.SoLuong),
                TrangThai = "ChoThanhToan",
                DiaChiGiaoHang = diaChiGiaoHang
            };

            _context.DonHang.Add(donHang);
            await _context.SaveChangesAsync();

            // Tạo chi tiết đơn hàng
            foreach (var item in cart)
            {
                var chiTiet = new ChiTietDonHang
                {
                    IdDonHang = donHang.IdDonHang,
                    IdSanPham = item.IdSanPham,
                    SoLuong = item.SoLuong,
                    DonGia = item.Gia
                };
                _context.ChiTietDonHang.Add(chiTiet);

                // Cập nhật số lượng tồn kho
                var sanPham = await _context.SanPham.FindAsync(item.IdSanPham);
                if (sanPham != null)
                {
                    sanPham.SoLuongTon -= item.SoLuong;
                }
            }

            await _context.SaveChangesAsync();

            // Xóa giỏ hàng
            HttpContext.Session.Remove("Cart");

            TempData["Success"] = "Đặt hàng thành công! Mã đơn hàng: " + donHang.IdDonHang;
            return RedirectToAction("OrderDetails", "Orders", new { area = "Customer", id = donHang.IdDonHang });
        }
    }
} 