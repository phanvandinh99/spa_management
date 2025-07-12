using Microsoft.AspNetCore.Mvc;
using SpaManagement.Web.Models;
using SpaManagement.Web.Models.EF;

namespace SpaManagement.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly SpaDbContext _context;

        public HomeController(SpaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy danh sách dịch vụ nổi bật
            var services = _context.DichVu.Take(3).ToList();
            
            // Lấy danh sách sản phẩm nổi bật
            var products = _context.SanPham.Take(4).ToList();

            ViewBag.Services = services;
            ViewBag.Products = products;

            return View();
        }

        public IActionResult Services()
        {
            var services = _context.DichVu.ToList();
            return View(services);
        }

        public IActionResult Products()
        {
            var products = _context.SanPham.ToList();
            return View(products);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
} 