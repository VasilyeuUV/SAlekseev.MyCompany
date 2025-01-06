using SAlekseev.MyCompany.Infrastructure;

namespace SAlekseev.MyCompany;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // ���������� � ������������ ���� appsettings.json
        IConfigurationBuilder configBuilder = new ConfigurationBuilder()
            .SetBasePath(builder.Environment.ContentRootPath)                               // - ����, � ������� ��������� ��� ����� ����������
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)         // - ���������� ���� appsettings.json (���������� ����������� � ����������� ��� ��� ���������)
            .AddEnvironmentVariables();                                                     // - ���������� ���������� ���������.

        // ����������� ������ Project � ��������� ����� ��� ��������
        IConfiguration configuration = configBuilder.Build();
        AppConfig config = configuration.GetSection("Project").Get<AppConfig>()!;           // - �������� ������ Project �� appsettings.json � ����� �� � ����� AppConfig

        // ���������� ���������� ������������ (��������� � ����� MVC)
        builder.Services.AddControllersWithViews();

        // �������� ������������
        WebApplication app = builder.Build();



        // !!! ������� ���������� middleware ����� �����. ��� ����� ����������� �������� ���� !!!

        // - ���������� ������������� ����� ��������� ������ (js, css, ...)? � �.�. � �� ����� wwwroot
        app.UseStaticFiles();

        // - ���������� ������� ������������� (����� ��������� �������������� �����������)
        app.UseRouting();

        // - ������������� ������ ��������
        app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");       // - ������� �� ���������

        await app.RunAsync();
    }
}
