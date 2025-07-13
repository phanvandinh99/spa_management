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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var order = await _context.DonHang.FindAsync(id);
            if (order == null) return NotFound();
            ViewBag.TrangThai = new SelectList(new[] { "ChoThanhToan", "DaThanhToan", "DangGiao", "DaGiao", "DaHuy" }, order.TrangThai);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DonHang donHang)
        {
            if (id != donHang.IdDonHang) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.DonHang.Any(e => e.IdDonHang == donHang.IdDonHang))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrangThai = new SelectList(new[] { "ChoThanhToan", "DaThanhToan", "DangGiao", "DaGiao", "DaHuy" }, donHang.TrangThai);
            return View(donHang);
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
            var order = await _context.DonHang.FindAsync(id);
            if (order != null)
            {
                _context.DonHang.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 