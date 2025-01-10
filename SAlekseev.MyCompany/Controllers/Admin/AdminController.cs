using System.Text.Json;
using System.Text.Json.Serialization;
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
    private readonly IWebHostEnvironment _hostingEnviroment;        // - для определения путей на сервере.
    private readonly ILogger<AdminController> _logger;

    /// <summary>
    /// CTOR
    /// </summary>
    public AdminController(
        DataManager dataManager, 
        IWebHostEnvironment hostingEnviroment,
        ILogger<AdminController> logger)
    {
        _dataManager = dataManager;
        _hostingEnviroment = hostingEnviroment;
        _logger = logger;
    }


    /// <summary>
    /// Главное действие по умолчанию.
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Index()
    {
        ViewBag.ServiceCategories = await _dataManager.ServiceCategories.GetServiceCategoriesAsync();
        ViewBag.Services = await _dataManager.Services.GetServicesAsync();
        return View();                          // - возврат одноименного контроллеру представления.
    }


    /// <summary>
    /// Метод для скрипта TinyMCE для сохранения изображений
    /// </summary>
    /// <returns></returns>
    public async Task<string> SaveEditorImg()
    {
        IFormFile img = Request.Form.Files[0];
        await SaveImg(img);

        return JsonSerializer.Serialize(new {location = Path.Combine("/img/", img.FileName)});      // - вернуть путь к файлу в браузер пользователя.
    }


    /// <summary>
    /// Сохранение картинок в файловую систему сайта.
    /// </summary>
    /// <param name="titleImageFile"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private async Task<string> SaveImg(IFormFile img)
    {
        string path = Path.Combine(_hostingEnviroment.WebRootPath, "img/", img.FileName);       // - формирование полного пути файла для сохранения на сервере.
        await using FileStream stream = new FileStream(path, FileMode.Create);
        await img.CopyToAsync(stream);                                                          // - сохраняем файл.

        return path;
    }
}
