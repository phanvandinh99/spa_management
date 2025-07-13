using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SpaManagement.Web.Models;
using SpaManagement.Web.Models.EF;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            ViewBag.IdDanhMuc = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(danhMucList, "IdDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPham sanPham, IFormFile? HinhAnh)
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
            // Nếu có lỗi, nạp lại danh mục để dropdown không bị rỗng
            var danhMucList = _context.DanhMucSanPham.ToList();
            ViewBag.IdDanhMuc = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(danhMucList, "IdDanhMuc", "TenDanhMuc", sanPham.IdDanhMuc);
            return View(sanPham);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham == null) return NotFound();
            var danhMucList = _context.DanhMucSanPham.ToList();
            ViewBag.IdDanhMuc = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(danhMucList, "IdDanhMuc", "TenDanhMuc", sanPham.IdDanhMuc);
            return View(sanPham);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SanPham sanPham, IFormFile? HinhAnh)
        {
            if (id != sanPham.IdSanPham) return NotFound();
            if (ModelState.IsValid)
            {
                try
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
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.SanPham.Any(e => e.IdSanPham == sanPham.IdSanPham))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            // Nếu có lỗi, nạp lại danh mục để dropdown không bị rỗng
            var danhMucList = _context.DanhMucSanPham.ToList();
            ViewBag.IdDanhMuc = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(danhMucList, "IdDanhMuc", "TenDanhMuc", sanPham.IdDanhMuc);
            return View(sanPham);
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
                _context.SanPham.Remove(sanPham);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 