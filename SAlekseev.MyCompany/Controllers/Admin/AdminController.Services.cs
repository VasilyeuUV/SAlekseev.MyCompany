using Microsoft.AspNetCore.Mvc;
using SAlekseev.MyCompany.Domain.Entities;

namespace SAlekseev.MyCompany.Controllers.Admin;

/// <summary>
/// Часть контролдлера для административной панели, работающая с функционалом для Услуг.
/// </summary>
public partial class AdminController
{
    /// <summary>
    /// Редактирование Услуги - получение исходных данных и открытие представления.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> ServicesEdit(int id)
    {
        // В зависимости от наличия id мы либо добавляем, либо изменяем запись.
        Service? entity = id == default
            ? new Service()
            : await _dataManager.Services.GetServiceByIdAsync(id);
        ViewBag.ServiceCategories = await _dataManager.ServiceCategories.GetServiceCategoriesAsync();   // - предварительно забираем категории
        return View(entity);
    }


    /// <summary>
    /// Post-версия Редактирования Услуги для сохранения
    /// </summary>
    /// <param name="entity">Редактируемая Услуга</param>
    /// <param name="titleImageFile"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ServicesEdit(Service entity, IFormFile? titleImageFile)
    {
        // В модеои присутствуют ошибки, то возвращаем на доработку
        if (!ModelState.IsValid)
        {
            ViewBag.ServiceCategories = await _dataManager.ServiceCategories.GetServiceCategoriesAsync();   // - предварительно забираем категории
            return View(entity);
        }

        if (titleImageFile != null)
        {
            entity.Photo = titleImageFile.FileName;
            await SaveImg(titleImageFile);
        }

        await _dataManager.Services.SaveServiceAsync(entity);
        return RedirectToAction("Index");                                               // - возвращаем на главную страницу админки.
    }


    [HttpPost]
    public async Task<IActionResult> ServicesDelete(int id)
    {
        await _dataManager.Services.DeleteServiceAsync(id);
        return RedirectToAction("Index");                                               // - возвращаем на главную страницу админки.
    }
}
