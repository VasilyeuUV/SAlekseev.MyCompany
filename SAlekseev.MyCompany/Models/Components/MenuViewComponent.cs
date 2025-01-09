using Microsoft.AspNetCore.Mvc;
using SAlekseev.MyCompany.Domain;
using SAlekseev.MyCompany.Domain.Entities;
using SAlekseev.MyCompany.Infrastructure;
using SAlekseev.MyCompany.Models.Dto;

namespace SAlekseev.MyCompany.Models.Components;


/// <summary>
/// Компонет для меню.
/// </summary>
public class MenuViewComponent : ViewComponent
{
    private readonly DataManager _dataManager;

    /// <summary>
    /// CTOR
    /// </summary>
    /// <param name="dataManager"></param>
    public MenuViewComponent(DataManager dataManager)
    {
        _dataManager = dataManager;
    }


    /// <summary>
    /// Асинхронный вызов вьюкомпонентов
    /// </summary>
    /// <returns></returns>
    public async Task<IViewComponentResult> InvokeAsync()
    {
        IEnumerable<Service> list = await _dataManager.Services.GetServicesAsync();     // - получаем из БД все услуги

        // Доменную сущность на клиенте использовать не рекомендуется. поэтому оборачиваем ее в DTO
        IEnumerable<ServiceDto> listDto = DtoHelper.TransformServices(list);

        return await Task.FromResult((IViewComponentResult)View("Default", listDto));   // - Default - имя вьюкомпонента по соглашению.
    }
}
