using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SpaManagement.Web.Models;
using SpaManagement.Web.Models.EF;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace SpaManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly SpaDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(SpaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.SanPham.Include(p => p.DanhMucSanPham).ToListAsync();
            return View(products);
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.SanPham.Include(p => p.DanhMucSanPham).FirstOrDefaultAsync(m => m.IdSanPham == id);
            if (product == null) return NotFound();
            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            var danhMucList = _context.DanhMucSanPham.ToList();
            ViewBag.IdDanhMuc = new SelectList(danhMucList, "IdDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPham sanPham, IFormFile? HinhAnh)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (HinhAnh != null && HinhAnh.Length > 0)
                    {
                        var uploads = Path.Combine(_env.WebRootPath, "images/products");
                        if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                        var fileName = Path.GetFileNameWithoutExtension(HinhAnh.FileName) + "_" + System.Guid.NewGuid().ToString().Substring(0, 8) + Path.GetExtension(HinhAnh.FileName);
                        var filePath = Path.Combine(uploads, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnh.CopyToAsync(stream);
                        }
                        sanPham.HinhAnhURL = "/images/products/" + fileName;
                    }
                    _context.Add(sanPham);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Log lỗi ModelState
                    foreach (var key in ModelState.Keys)
                    {
                        var errors = ModelState[key].Errors;
                        foreach (var error in errors)
                        {
                            System.Diagnostics.Debug.WriteLine($"ModelState Error for {key}: {error.ErrorMessage}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in Create POST: {ex.Message}\n{ex.StackTrace}");
                ModelState.AddModelError("", "Đã xảy ra lỗi: " + ex.Message);
            }
            var danhMucList = _context.DanhMucSanPham.ToList();
            ViewBag.IdDanhMuc = new SelectList(danhMucList, "IdDanhMuc", "TenDanhMuc", sanPham.IdDanhMuc);
            return View(sanPham);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham == null) return NotFound();
            var danhMucList = _context.DanhMucSanPham.ToList();
            ViewBag.IdDanhMuc = new SelectList(danhMucList, "IdDanhMuc", "TenDanhMuc", sanPham.IdDanhMuc);
            return View(sanPham);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(IFormCollection form, IFormFile? HinhAnh)
        {
            var idSanPhamStr = form["IdSanPham"];
            var tenSanPham = form["TenSanPham"];
            var idDanhMucStr = form["IdDanhMuc"];
            var giaStr = form["Gia"];
            var soLuongTonStr = form["SoLuongTon"];
            var moTa = form["MoTa"];

            if (!int.TryParse(idSanPhamStr, out int idSanPham))
                return BadRequest();

            var sanPham = await _context.SanPham.FindAsync(idSanPham);
            if (sanPham == null)
                return NotFound();

            sanPham.TenSanPham = tenSanPham;
            sanPham.MoTa = moTa;
            sanPham.IdDanhMuc = int.TryParse(idDanhMucStr, out int idDanhMuc) ? idDanhMuc : sanPham.IdDanhMuc;
            sanPham.Gia = decimal.TryParse(giaStr, out decimal gia) ? gia : sanPham.Gia;
            sanPham.SoLuongTon = int.TryParse(soLuongTonStr, out int soLuongTon) ? soLuongTon : sanPham.SoLuongTon;

            if (HinhAnh != null && HinhAnh.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "images/products");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                var fileName = Path.GetFileNameWithoutExtension(HinhAnh.FileName) + "_" + Guid.NewGuid().ToString().Substring(0, 8) + Path.GetExtension(HinhAnh.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await HinhAnh.CopyToAsync(stream);
                }
                sanPham.HinhAnhURL = "/images/products/" + fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.SanPham.Include(p => p.DanhMucSanPham).FirstOrDefaultAsync(m => m.IdSanPham == id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham != null)
            {
                try
                {
                    _context.SanPham.Remove(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE constraint"))
                    {
                        TempData["Error"] = "Không thể xóa sản phẩm vì đã có đơn hàng liên quan!";
                        return RedirectToAction(nameof(Delete), new { id });
                    }
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}