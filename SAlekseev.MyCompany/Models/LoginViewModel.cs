using System.ComponentModel.DataAnnotations;

namespace SAlekseev.MyCompany.Models;

/// <summary>
/// Модель для представления окна авторизации на сайте. Транспортируют данные между контроллером и представлением.
/// </summary>
public class LoginViewModel
{
    [Required]
    [Display(Name = "Логин")]
    public string? UserName { get; set; }

    [Required]
    [UIHint("password")]                                    // - обозначает, что этот input будет иметь type="password"
    [Display(Name = "Пароль")]
    public string? Password { get; set; }

    /// <summary>
    /// Для галочки "Запомнить меня"
    /// </summary>
    [Display(Name = "Запомнить меня")]
    public bool RememberMe { get; set; }

}
