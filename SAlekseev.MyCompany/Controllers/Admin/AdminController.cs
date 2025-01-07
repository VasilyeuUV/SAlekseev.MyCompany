using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAlekseev.MyCompany.Domain;

namespace SAlekseev.MyCompany.Controllers.Admin;


/// <summary>
/// Контроллер для административной панели
/// </summary>
[Authorize(Roles = "admin")]                               // - Защищенная зона. Атрибут указывает, что нужно пройти авторизацию. И сюда могут попасть Пользователи только с ролью "admin"
public partial class AdminController : Controller
{
    private readonly DataManager _dataManager;


    /// <summary>
    /// CTOR
    /// </summary>
    public AdminController(DataManager dataManager)
    {
        _dataManager = dataManager;
    }


    /// <summary>
    /// Главное действие по умолчанию.
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        return View();                          // - возврат одноименного контроллеру представления.
    }
}
