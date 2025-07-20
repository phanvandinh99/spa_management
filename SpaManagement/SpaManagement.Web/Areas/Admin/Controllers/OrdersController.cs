using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaManagement.Web.Models;
using SpaManagement.Web.Models.EF;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SpaManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly SpaDbContext _context;
        public OrdersController(SpaDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.DonHang
                .Include(o => o.KhachHang)
                .Include(o => o.NhanVien)
                .Include(o => o.KhuyenMai)
                .OrderByDescending(o => o.NgayDatHang)
                .ToListAsync();
            return View(orders);
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var order = await _context.DonHang
                .Include(o => o.KhachHang)
                .Include(o => o.NhanVien)
                .Include(o => o.KhuyenMai)
                .Include(o => o.ChiTietDonHangs).ThenInclude(c => c.SanPham)
                .FirstOrDefaultAsync(m => m.IdDonHang == id);
            if (order == null) return NotFound();
            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> EditStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.DonHang
                .Include(o => o.KhachHang)
                .FirstOrDefaultAsync(o => o.IdDonHang == id);
            if (order == null) return NotFound();
            var trangThaiList = new[]
            {
                new { Value = "ChoThanhToan", Text = "Chờ thanh toán" },
                new { Value = "DangXuLy", Text = "Đang xử lý" },
                new { Value = "DangGiao", Text = "Đang giao" },
                new { Value = "DaGiao", Text = "Đã giao" },
                new { Value = "DaHuy", Text = "Đã hủy" }
            };
            ViewBag.TrangThai = new SelectList(trangThaiList, "Value", "Text", order.TrangThai);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(int IdDonHang, string TrangThai)
        {
            var donHang = await _context.DonHang
                .Include(o => o.KhachHang)
                .FirstOrDefaultAsync(o => o.IdDonHang == IdDonHang);
            if (donHang == null) return NotFound();

            donHang.TrangThai = TrangThai;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.DonHang.Any(e => e.IdDonHang == IdDonHang))
                    return NotFound();
                else
                    throw;
            }
            // Nếu có lỗi, lấy lại danh sách trạng thái và return View với model đầy đủ
            var trangThaiList = new[]
            {
                new { Value = "ChoThanhToan", Text = "Chờ thanh toán" },
                new { Value = "DangXuLy", Text = "Đang xử lý" },
                new { Value = "DangGiao", Text = "Đang giao" },
                new { Value = "DaGiao", Text = "Đã giao" },
                new { Value = "DaHuy", Text = "Đã hủy" }
            };
            ViewBag.TrangThai = new SelectList(trangThaiList, "Value", "Text", donHang.TrangThai);
            return View("EditStatus", donHang);
        }

        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var order = await _context.DonHang
                .Include(o => o.KhachHang)
                .FirstOrDefaultAsync(m => m.IdDonHang == id);
            if (order == null) return NotFound();
            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.DonHang
                .Include(o => o.ChiTietDonHangs)
                .Include(o => o.ThanhToans)
                .FirstOrDefaultAsync(o => o.IdDonHang == id);

            if (order != null)
            {
                // Xóa chi tiết đơn hàng
                _context.ChiTietDonHang.RemoveRange(order.ChiTietDonHangs);
                // Xóa thanh toán liên quan
                _context.ThanhToan.RemoveRange(order.ThanhToans);
                // Xóa đơn hàng
                _context.DonHang.Remove(order);

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}