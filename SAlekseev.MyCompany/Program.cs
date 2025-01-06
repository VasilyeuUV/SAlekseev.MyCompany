using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SAlekseev.MyCompany.Domain;
using SAlekseev.MyCompany.Domain.Users;
using SAlekseev.MyCompany.Infrastructure;

namespace SAlekseev.MyCompany;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Подключаем в конфигурацию файл appsettings.json
        IConfigurationBuilder configBuilder = new ConfigurationBuilder()
            .SetBasePath(builder.Environment.ContentRootPath)                               // - путь, в котором находятся все файлы приложения
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)         // - подключаем файл appsettings.json (подключать обязательно и перегружать при его изменении)
            .AddEnvironmentVariables();                                                     // - подключаем переменные окружения.

        // Оборачиваем секцию Project в объектную форму для удобства
        IConfiguration configuration = configBuilder.Build();
        AppConfig config = configuration.GetSection("Project").Get<AppConfig>()!;           // - забираем секцию Project из appsettings.json и мапим ее в класс AppConfig

        // Подключение контекста БД
        builder.Services.AddDbContext<AppDbContext>(x =>
            x.UseSqlServer(config.DataBase.ConnectionString)
            .ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));            // - в версии EF Core 9.0.0 был баг без ошибки, поэтому подавляем (игнорируем) предупреждения

        // Настраиваем систему идентификации (чтобы можно было залогиниться)
        builder.Services.AddIdentity<CustomIdentityUser, CustomIdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;                                     // - обязательно уникальный email для каждого Пользователя
                options.Password.RequiredLength = 6;                                        // - минимальная длина пароля
                options.Password.RequireNonAlphanumeric = false;                            // - использование специальных символов в пароле не обязательно
                options.Password.RequireLowercase = false;                                  // - использование в пароле символов в нижнем регистре не обязательно 
                options.Password.RequireUppercase = false;                                  // - использование в пароле символов в верхнем регистре не обязательно 
                options.Password.RequireDigit = false;                                      // - использование в пароле цифр не обязательно 
            })
            .AddEntityFrameworkStores<AppDbContext>()                                       // -
            .AddDefaultTokenProviders();                                                    // - 

        // Настраиваем cookie для аутентификации
        builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "myCompanyAuth";                  // - название куки
                options.Cookie.HttpOnly = true;                         // - к этим кукам запрещен доступ из сторонних средств, например js-скриптов в браузере пользователя, т.к. эти куки отвечают за безопасность, а только по http
                options.LoginPath = "/admin/login";                     // - прежде чем попасть в панель администратора, необходимо залогинится. Этот путь открывает страницу, чтобы залогинится. Логин/пароль были заданы в appsetting.json и использованы при создании базы данных.
                options.AccessDeniedPath = "/admin/accessdenied";       // - 
                options.SlidingExpiration = true;                       // - 
            });



        // Подключаем функционал контроллеров (переходим в режим MVC)
        builder.Services.AddControllersWithViews();

        // Собираем конфигурацию
        WebApplication app = builder.Build();



        // !!! Порядок следования middleware очень важен. они будут выполняться согласно нему !!!

        // - Подключаем использование любых статичных файлов (js, css, ...)? в т.ч. и из папки wwwroot
        app.UseStaticFiles();

        // - Подключаем систему маршрутизации (чтобы корректно обрабатывались контроллеры)
        app.UseRouting();

        // - Подключаем аутентификацию и авторизацию
        app.UseCookiePolicy();                                          // - подключение политики для куки
        app.UseAuthentication();                                        // - подключение аутентификации для Пользователя
        app.UseAuthorization();                                         // - подключение авторизации для Пользователя

        // - Регистирируем нужные маршруты
        app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");       // - маршрут по умолчанию

        await app.RunAsync();
    }
}
