using Microsoft.AspNetCore.Mvc;
using SAlekseev.MyCompany.Domain.Entities;

namespace SAlekseev.MyCompany.Controllers.Admin;

/// <summary>
/// Часть контролдлера для административной панели, работающая с функционалом для Категории услуг.
/// </summary>
public partial class AdminController
{
    /// <summary>
    /// Действие при создании/редактировании Категории услуг.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> ServiceCategoriesEdit(int id)
    {
        // В зависимости от наличия id мы либо добавляем, либо изменяем запись.
        ServiceCategory? entity = id == default
            ? new ServiceCategory()
            : await _dataManager.ServiceCategories.GetServiceCategoryByIdAsync(id);
        return View(entity);
    }


    [HttpPost]
    public async Task<IActionResult> ServiceCategoriesEdit(ServiceCategory entity)
    {
        // В модеои присутствуют ошибки, то возвращаем на доработку
        if (!ModelState.IsValid)
        {
            return View(entity);
        }

        await _dataManager.ServiceCategories.SaveServiceCategoryAsync(entity);
        return RedirectToAction("Index");                                               // - возвращаем на главную страницу админки.
    }


    [HttpPost]
    public async Task<IActionResult> ServiceCategoriesDelete(int id)
    {
        // TODO: Т.к. в целях юезопасности отключено каскадное удаление, то прежде чем удалить категорию, убедимся, что на нее нет ссылки ни у одной из услуг

        await _dataManager.ServiceCategories.DeleteServiceCategoryAsync(id);
        return RedirectToAction("Index");                                               // - возвращаем на главную страницу админки.
    }

}
