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
    public class BookingController : Controller
    {
        private readonly SpaDbContext _context;

        public BookingController(SpaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? serviceId)
        {
            var services = await _context.DichVu.ToListAsync();
            var employees = await _context.NhanVien.ToListAsync();
            ViewBag.Services = services;
            ViewBag.Employees = employees;
            ViewBag.SelectedServiceId = serviceId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int IdDichVu, DateTime ThoiGianBatDau, int? IdNhanVien, string? GhiChu)
        {
            // Lấy IdKhachHang từ tài khoản đăng nhập
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var khachHang = await _context.KhachHang.FirstOrDefaultAsync(kh => kh.TaiKhoan != null && kh.TaiKhoan.TenDangNhap == User.Identity.Name);
            if (khachHang == null)
            {
                ModelState.AddModelError("", "Không tìm thấy thông tin khách hàng.");
            }
            else if (ThoiGianBatDau < DateTime.Now)
            {
                ModelState.AddModelError("", "Thời gian đặt lịch phải lớn hơn hiện tại.");
            }
            else
            {
                var dichVu = await _context.DichVu.FindAsync(IdDichVu);
                if (dichVu == null)
                {
                    ModelState.AddModelError("", "Dịch vụ không tồn tại.");
                }
                else
                {
                    var lichHen = new LichHen
                    {
                        IdKhachHang = khachHang.IdKhachHang,
                        IdDichVu = IdDichVu,
                        IdNhanVien = IdNhanVien,
                        ThoiGianBatDau = ThoiGianBatDau,
                        ThoiGianKetThuc = ThoiGianBatDau.AddMinutes(dichVu.ThoiLuongPhut ?? 60),
                        TrangThai = "DaDat",
                        GhiChu = GhiChu
                    };
                    _context.LichHen.Add(lichHen);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Đặt lịch thành công!";
                    return RedirectToAction("Index");
                }
            }
            var services = await _context.DichVu.ToListAsync();
            var employees = await _context.NhanVien.ToListAsync();
            ViewBag.Services = services;
            ViewBag.Employees = employees;
            ViewBag.SelectedServiceId = IdDichVu;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyLich(int id)
        {
            var userName = User.Identity.Name;
            var khachHang = await _context.KhachHang.Include(kh => kh.TaiKhoan)
                .FirstOrDefaultAsync(kh => kh.TaiKhoan != null && kh.TaiKhoan.TenDangNhap == userName);
            if (khachHang == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Customer" });
            }
            var lichHen = await _context.LichHen.FirstOrDefaultAsync(lh => lh.IdLichHen == id && lh.IdKhachHang == khachHang.IdKhachHang);
            if (lichHen == null || lichHen.TrangThai != "DaDat")
            {
                TempData["Error"] = "Không thể hủy lịch này.";
                return RedirectToAction("MyBookings");
            }
            if ((lichHen.ThoiGianBatDau - DateTime.Now).TotalMinutes < 60)
            {
                TempData["Error"] = "Chỉ được hủy lịch trước giờ hẹn ít nhất 1 tiếng.";
                return RedirectToAction("MyBookings");
            }
            lichHen.TrangThai = "DaHuy";
            await _context.SaveChangesAsync();
            TempData["Success"] = "Hủy lịch thành công.";
            return RedirectToAction("MyBookings");
        }

        [HttpGet]
        public async Task<IActionResult> MyBookings()
        {
            var userName = User.Identity.Name;
            var khachHang = await _context.KhachHang.Include(kh => kh.TaiKhoan)
                .FirstOrDefaultAsync(kh => kh.TaiKhoan != null && kh.TaiKhoan.TenDangNhap == userName);
            if (khachHang == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Customer" });
            }
            var lichHens = await _context.LichHen
                .Include(lh => lh.DichVu)
                    .ThenInclude(dv => dv.DanhGias)
                .Include(lh => lh.NhanVien)
                .Where(lh => lh.IdKhachHang == khachHang.IdKhachHang)
                .OrderByDescending(lh => lh.ThoiGianBatDau)
                .ToListAsync();
            return View(lichHens);
        }

        [HttpGet]
        public async Task<IActionResult> DanhGia(int id)
        {
            var userName = User.Identity.Name;
            var khachHang = await _context.KhachHang.Include(kh => kh.TaiKhoan)
                .FirstOrDefaultAsync(kh => kh.TaiKhoan != null && kh.TaiKhoan.TenDangNhap == userName);
            if (khachHang == null)
                return RedirectToAction("Login", "Account", new { area = "Customer" });
            var lichHen = await _context.LichHen.Include(lh => lh.DichVu)
                .FirstOrDefaultAsync(lh => lh.IdLichHen == id && lh.IdKhachHang == khachHang.IdKhachHang);
            if (lichHen == null || lichHen.TrangThai != "DaHoanThanh")
                return RedirectToAction("MyBookings");
            // Kiểm tra đã đánh giá chưa
            var daDanhGia = await _context.DanhGia.AnyAsync(dg => dg.IdKhachHang == khachHang.IdKhachHang && dg.IdDichVu == lichHen.IdDichVu);
            if (daDanhGia)
                return RedirectToAction("MyBookings");
            ViewBag.DichVu = lichHen.DichVu;
            ViewBag.LichHenId = lichHen.IdLichHen;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DanhGia(int LichHenId, int SoSao, string? BinhLuan)
        {
            var userName = User.Identity.Name;
            var khachHang = await _context.KhachHang.Include(kh => kh.TaiKhoan)
                .FirstOrDefaultAsync(kh => kh.TaiKhoan != null && kh.TaiKhoan.TenDangNhap == userName);
            if (khachHang == null)
                return RedirectToAction("Login", "Account", new { area = "Customer" });
            var lichHen = await _context.LichHen.Include(lh => lh.DichVu)
                .FirstOrDefaultAsync(lh => lh.IdLichHen == LichHenId && lh.IdKhachHang == khachHang.IdKhachHang);
            if (lichHen == null || lichHen.TrangThai != "DaHoanThanh")
                return RedirectToAction("MyBookings");
            // Kiểm tra đã đánh giá chưa
            var daDanhGia = await _context.DanhGia.AnyAsync(dg => dg.IdKhachHang == khachHang.IdKhachHang && dg.IdDichVu == lichHen.IdDichVu);
            if (daDanhGia)
                return RedirectToAction("MyBookings");
            if (SoSao < 1 || SoSao > 5)
            {
                ModelState.AddModelError("", "Số sao phải từ 1 đến 5.");
                ViewBag.DichVu = lichHen.DichVu;
                ViewBag.LichHenId = lichHen.IdLichHen;
                return View();
            }
            var danhGia = new DanhGia
            {
                IdKhachHang = khachHang.IdKhachHang,
                IdDichVu = lichHen.IdDichVu,
                SoSao = SoSao,
                BinhLuan = BinhLuan,
                NgayDanhGia = DateTime.Now
            };
            _context.DanhGia.Add(danhGia);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đánh giá thành công!";
            return RedirectToAction("MyBookings");
        }
    }
} 