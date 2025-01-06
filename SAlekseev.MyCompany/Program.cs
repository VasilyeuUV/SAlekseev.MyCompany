using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SAlekseev.MyCompany.Domain;
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

        // Подключаем функционал контроллеров (переходим в режим MVC)
        builder.Services.AddControllersWithViews();

        // Собираем конфигурацию
        WebApplication app = builder.Build();



        // !!! Порядок следования middleware очень важен. они будут выполняться согласно нему !!!

        // - Подключаем использование любых статичных файлов (js, css, ...)? в т.ч. и из папки wwwroot
        app.UseStaticFiles();

        // - Подключаем систему маршрутизации (чтобы корректно обрабатывались контроллеры)
        app.UseRouting();

        // - Регистирируем нужные маршруты
        app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");       // - маршрут по умолчанию

        await app.RunAsync();
    }
}
