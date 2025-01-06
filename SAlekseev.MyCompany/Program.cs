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

        // ���������� � ������������ ���� appsettings.json
        IConfigurationBuilder configBuilder = new ConfigurationBuilder()
            .SetBasePath(builder.Environment.ContentRootPath)                               // - ����, � ������� ��������� ��� ����� ����������
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)         // - ���������� ���� appsettings.json (���������� ����������� � ����������� ��� ��� ���������)
            .AddEnvironmentVariables();                                                     // - ���������� ���������� ���������.

        // ����������� ������ Project � ��������� ����� ��� ��������
        IConfiguration configuration = configBuilder.Build();
        AppConfig config = configuration.GetSection("Project").Get<AppConfig>()!;           // - �������� ������ Project �� appsettings.json � ����� �� � ����� AppConfig

        // ����������� ��������� ��
        builder.Services.AddDbContext<AppDbContext>(x =>
            x.UseSqlServer(config.DataBase.ConnectionString)
            .ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));            // - � ������ EF Core 9.0.0 ��� ��� ��� ������, ������� ��������� (����������) ��������������

        // ����������� ������� ������������� (����� ����� ���� ������������)
        builder.Services.AddIdentity<CustomIdentityUser, CustomIdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;                                     // - ����������� ���������� email ��� ������� ������������
                options.Password.RequiredLength = 6;                                        // - ����������� ����� ������
                options.Password.RequireNonAlphanumeric = false;                            // - ������������� ����������� �������� � ������ �� �����������
                options.Password.RequireLowercase = false;                                  // - ������������� � ������ �������� � ������ �������� �� ����������� 
                options.Password.RequireUppercase = false;                                  // - ������������� � ������ �������� � ������� �������� �� ����������� 
                options.Password.RequireDigit = false;                                      // - ������������� � ������ ���� �� ����������� 
            })
            .AddEntityFrameworkStores<AppDbContext>()                                       // -
            .AddDefaultTokenProviders();                                                    // - 

        // ����������� cookie ��� ��������������
        builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "myCompanyAuth";                  // - �������� ����
                options.Cookie.HttpOnly = true;                         // - � ���� ����� �������� ������ �� ��������� �������, �������� js-�������� � �������� ������������, �.�. ��� ���� �������� �� ������������, � ������ �� http
                options.LoginPath = "/admin/login";                     // - ������ ��� ������� � ������ ��������������, ���������� �����������. ���� ���� ��������� ��������, ����� �����������. �����/������ ���� ������ � appsetting.json � ������������ ��� �������� ���� ������.
                options.AccessDeniedPath = "/admin/accessdenied";       // - 
                options.SlidingExpiration = true;                       // - 
            });



        // ���������� ���������� ������������ (��������� � ����� MVC)
        builder.Services.AddControllersWithViews();

        // �������� ������������
        WebApplication app = builder.Build();



        // !!! ������� ���������� middleware ����� �����. ��� ����� ����������� �������� ���� !!!

        // - ���������� ������������� ����� ��������� ������ (js, css, ...)? � �.�. � �� ����� wwwroot
        app.UseStaticFiles();

        // - ���������� ������� ������������� (����� ��������� �������������� �����������)
        app.UseRouting();

        // - ���������� �������������� � �����������
        app.UseCookiePolicy();                                          // - ����������� �������� ��� ����
        app.UseAuthentication();                                        // - ����������� �������������� ��� ������������
        app.UseAuthorization();                                         // - ����������� ����������� ��� ������������

        // - ������������� ������ ��������
        app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");       // - ������� �� ���������

        await app.RunAsync();
    }
}
