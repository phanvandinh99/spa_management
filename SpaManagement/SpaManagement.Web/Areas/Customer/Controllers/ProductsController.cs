using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaManagement.Web.Models.EF;
using SpaManagement.Web.Models;

namespace SpaManagement.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductsController : Controller
    {
        private readonly SpaDbContext _context;

        public ProductsController(SpaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.SanPham
                .Include(sp => sp.DanhMucSanPham)
                .Include(sp => sp.DanhGias)
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.SanPham
                .Include(sp => sp.DanhMucSanPham)
                .Include(sp => sp.DanhGias)
                    .ThenInclude(dg => dg.KhachHang)
                .FirstOrDefaultAsync(sp => sp.IdSanPham == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> Category(int? categoryId)
        {
            var query = _context.SanPham
                .Include(sp => sp.DanhMucSanPham)
                .Include(sp => sp.DanhGias)
                .AsQueryable();
            if (categoryId.HasValue)
            {
                query = query.Where(sp => sp.IdDanhMuc == categoryId);
            }
            var products = await query.ToListAsync();
            var categories = await _context.DanhMucSanPham.ToListAsync();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = categoryId;
            return View("Index", products);
        }
    }
} 