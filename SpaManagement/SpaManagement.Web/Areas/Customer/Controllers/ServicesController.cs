using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaManagement.Web.Models.EF;
using SpaManagement.Web.Models;

namespace SpaManagement.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ServicesController : Controller
    {
        private readonly SpaDbContext _context;

        public ServicesController(SpaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.DichVu
                .Include(dv => dv.DanhMucDichVu)
                .Include(dv => dv.DanhGias)
                .ToListAsync();

            return View(services);
        }

        public async Task<IActionResult> Details(int id)
        {
            var service = await _context.DichVu
                .Include(dv => dv.DanhMucDichVu)
                .Include(dv => dv.DanhGias)
                    .ThenInclude(dg => dg.KhachHang)
                .FirstOrDefaultAsync(dv => dv.IdDichVu == id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        public async Task<IActionResult> Category(int? categoryId)
        {
            var query = _context.DichVu
                .Include(dv => dv.DanhMucDichVu)
                .Include(dv => dv.DanhGias)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(dv => dv.IdDanhMuc == categoryId);
            }

            var services = await query.ToListAsync();
            var categories = await _context.DanhMucDichVu.ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = categoryId;

            return View("Index", services);
        }
    }
} 