using Microsoft.AspNetCore.Mvc;

namespace SpaManagement.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 