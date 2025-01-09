using Microsoft.AspNetCore.Mvc;

namespace SAlekseev.MyCompany.Controllers;

public class HomeController : Controller
{
    /// <summary>
    /// Главная страница.
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        return View();
    }


    /// <summary>
    /// Страница с Контактами
    /// </summary>
    /// <returns></returns>
    public IActionResult Contacts()
    {
        return View();
    }

}
