using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SAlekseev.MyCompany.Domain;
using SAlekseev.MyCompany.Domain.Entities;
using SAlekseev.MyCompany.Infrastructure;
using SAlekseev.MyCompany.Models.Dto;

namespace SAlekseev.MyCompany.Controllers;

/// <summary>
/// Контроллер для Сервисов.
/// </summary>
public class ServicesController : Controller
{
    private readonly DataManager _dataManager;


    /// <summary>
    /// CTOR
    /// </summary>
    public ServicesController(DataManager dataManager)
    {
        _dataManager = dataManager;
    }


    /// <summary>
    /// Главная страница Сервисов.
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Index()
    {
        IEnumerable<Service> list = await _dataManager.Services.GetServicesAsync();

        // Доменную сущность на клиенте не используем.
        IEnumerable<ServiceDto> listDto = DtoHelper.TransformServices(list);

        return View(listDto);
    }


    /// <summary>
    /// Страница выбранной Услуги
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Show(int id)
    {
        Service? entity = await _dataManager.Services.GetServiceByIdAsync(id);

        // Если услуги с данным Id не найдено, отвечаем кодом 404 
        if (entity == null)
        {
            return NotFound();
        }

        // Доменную сущность на клиенте не используем.
        ServiceDto entityDto = DtoHelper.TransformService(entity);

        return View(entityDto);
    }
}
