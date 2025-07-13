using Microsoft.AspNetCore.Mvc;

namespace SpaManagement.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 