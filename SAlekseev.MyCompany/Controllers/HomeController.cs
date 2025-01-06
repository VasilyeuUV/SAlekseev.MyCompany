using Microsoft.AspNetCore.Mvc;

namespace SAlekseev.MyCompany.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
