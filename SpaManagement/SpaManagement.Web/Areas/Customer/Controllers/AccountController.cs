using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SpaManagement.Web.Models.EF;
using SpaManagement.Web.Models;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace SpaManagement.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        private readonly SpaDbContext _context;
        public AccountController(SpaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string TenDangNhap, string MatKhau, string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(TenDangNhap) || string.IsNullOrWhiteSpace(MatKhau))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                return View();
            }
            var user = await _context.TaiKhoan.Include(tk => tk.VaiTro)
                .FirstOrDefaultAsync(tk => tk.TenDangNhap == TenDangNhap && tk.MatKhauHash == HashPassword(MatKhau));
            if (user == null || user.VaiTro == null || user.VaiTro.TenVaiTro != "KhachHang")
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View();
            }
            // Đăng nhập thành công
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim(ClaimTypes.NameIdentifier, user.IdTaiKhoan.ToString()),
                new Claim(ClaimTypes.Role, user.VaiTro.TenVaiTro)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string HoTen, string SoDienThoai, string Email, string TenDangNhap, string MatKhau, string XacNhanMatKhau)
        {
            if (string.IsNullOrWhiteSpace(HoTen) || string.IsNullOrWhiteSpace(SoDienThoai) || string.IsNullOrWhiteSpace(Email)
                || string.IsNullOrWhiteSpace(TenDangNhap) || string.IsNullOrWhiteSpace(MatKhau) || string.IsNullOrWhiteSpace(XacNhanMatKhau))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                return View();
            }
            if (MatKhau != XacNhanMatKhau)
            {
                ModelState.AddModelError("", "Mật khẩu xác nhận không khớp.");
                return View();
            }
            if (await _context.TaiKhoan.AnyAsync(tk => tk.TenDangNhap == TenDangNhap))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
                return View();
            }
            if (await _context.KhachHang.AnyAsync(kh => kh.Email == Email))
            {
                ModelState.AddModelError("", "Email đã được sử dụng.");
                return View();
            }
            if (await _context.KhachHang.AnyAsync(kh => kh.SoDienThoai == SoDienThoai))
            {
                ModelState.AddModelError("", "Số điện thoại đã được sử dụng.");
                return View();
            }
            var role = await _context.VaiTro.FirstOrDefaultAsync(r => r.TenVaiTro == "KhachHang");
            if (role == null)
            {
                ModelState.AddModelError("", "Không tìm thấy vai trò khách hàng.");
                return View();
            }
            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = TenDangNhap,
                MatKhauHash = HashPassword(MatKhau),
                IdVaiTro = role.IdVaiTro,
                TrangThai = "HoatDong"
            };
            _context.TaiKhoan.Add(taiKhoan);
            await _context.SaveChangesAsync();
            var khachHang = new KhachHang
            {
                HoTen = HoTen,
                SoDienThoai = SoDienThoai,
                Email = Email,
                IdTaiKhoan = taiKhoan.IdTaiKhoan
            };
            _context.KhachHang.Add(khachHang);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đăng ký thành công! Bạn có thể đăng nhập.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
} 