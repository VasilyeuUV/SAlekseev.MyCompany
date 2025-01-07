using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAlekseev.MyCompany.Domain.Users;
using SAlekseev.MyCompany.Models;

namespace SAlekseev.MyCompany.Controllers;

/// <summary>
/// Контроллер для окна авторизации.
/// </summary>
public class AccountController : Controller
{
    private readonly SignInManager<CustomIdentityUser> _signInManager;          // - чтобы совершить Logout


    /// <summary>
    /// CTOR
    /// </summary>
    public AccountController(SignInManager<CustomIdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }


    /// <summary>
    /// Вход в авторизованную зону: открытие страницы авторизации.
    /// </summary>
    /// <param name="returnUrl">Адрес, на который хотим попасть после успешной авторизации. Приходит из адресной строки браузера.</param>
    /// <returns></returns>
    [HttpGet]                                         // - т.к. просто переходим на страницу логина.
    public async Task<IActionResult> Login(string? returnUrl)
    {
        // Метод асинхронный, т.к. целенаправленно завершаем текущий сеанс работы перед новым логином.
        await _signInManager.SignOutAsync();         // - выход из текущего сеанса, чтобы не было проблем с новым входом на сайт.

        // - передаем returnUrl в представление (т.е. какую страницу нужно потом открыть)
        ViewBag.ReturnUrl = returnUrl;               // - ViewBag - стандартный контейнер для передачи данных между контроллером и представлением
                                                     // - ReturnUrl - динамическое свойство (dynamic), которое передается.

        return View(new LoginViewModel());           // - возвращаем представление с таким же названием как и Контроллер, передав туда модель представления для логгирования.
    }


    /// <summary>
    /// Вход в авторизованную зону: открытие страницы административной панели.
    /// </summary>
    /// <param name="model">Логин/пароль.</param>
    /// <param name="returnUrl">Адрес административной панели (куда хотим попасть).</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
    {
        // При нажатии кнопки "Войти", все данные из аргументов отправляются на сервер.

        // Раздел сайта (URL), куда перенаправить пользователя после успешного логина.
        ViewBag.ReturnUrl = returnUrl;

        // 1-й этап проверки: на валидность модели.
        if (!ModelState.IsValid)                                                    // - если состояние модели не валидное
        {
            return View(model);                                                     // - возвращаем модель в представление на повторную попытку. 
        }

        // 2-й этап проверки: есть ли такой пользователь в БД и подходит ли пароль.

        // - Результат авторизации. Передаем данные для проверки.
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager
            .PasswordSignInAsync(
                model.UserName!,
                model.Password!,
                model.RememberMe,
                lockoutOnFailure: false
                );

        if (!result.Succeeded)                                                          // - если логин или пароль не верны, 
        {
            ModelState.AddModelError(string.Empty, "Неверный логин и пароль");          // - добавляем ошибку для модели
            return View(model);                                                         // - возвращаем модель на доработку.
        }

        return Redirect(returnUrl ?? "/");                                              // - если url не задан, возвращаемся на главную страницу.
    }


    /// <summary>
    /// Выйти из авторизованной зоны.
    /// </summary>
    /// <returns></returns>
    [HttpPost]                                                                  // - используется при изменении состояния сайта при добавлении/изменении данных
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();                                    // - выход из авторизованной зоны.
        return RedirectToAction("Index", "Home");                               // - после выхода из авторизованной зоны возвращаемся на главную страницу.
    }



}
