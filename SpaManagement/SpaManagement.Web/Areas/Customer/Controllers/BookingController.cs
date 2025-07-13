using Microsoft.AspNetCore.Mvc;

namespace SpaManagement.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 