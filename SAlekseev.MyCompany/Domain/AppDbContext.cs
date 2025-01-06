using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAlekseev.MyCompany.Domain.Entities;
using SAlekseev.MyCompany.Domain.Users;

namespace SAlekseev.MyCompany.Domain;

/// <summary>
/// Контекст базы данных сайта.
/// </summary>
public class AppDbContext : IdentityDbContext<CustomIdentityUser>     // - CustomIdentityUser - класс для Пользователя с конкретными требованиями для него, унаследованный от Стандартного IdentityUser
{
    /// <summary>
    /// CTOR
    /// </summary>
    /// <param name="options"></param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ServiceCategory> ServiceCategories { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;


    /// <summary>
    /// Настройки.
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // При создании базы данных создаем одну единственную роль для Администратора базы данных и одного едиственного Пользователя.
        string adminName = "admin";                                             // - имя администратора
        string roleAdminId = "7db0e6f6-6a21-48e2-874c-fb978325825c";            // - идентификатор роли администратора
        string userAdminId = "3d9786c5-38cf-45fa-8258-0e598384df2b";            // - идентификатор администратора

        // - добавляем роль администратора
        builder.Entity<CustomIdentityRole>()
            .HasData(new CustomIdentityRole()
            {
                Id = roleAdminId,
                Name = adminName,
                NormalizedName = adminName.ToUpper(),
            });

        // - добавляем нового Пользователя.
        builder.Entity<CustomIdentityUser>()
            .HasData(new CustomIdentityUser()
            {
                Id = userAdminId,                                           // - идентификатор Пользователя
                UserName = adminName,                                       // - имя Пользователя как Администратора
                NormalizedUserName = adminName.ToUpper(),                   // - нормальзованное имя Пользователя
                Email = "admin@admin.com",                                  // - электронная почта Администратора
                NormalizedEmail = "admin@admin.com",                        // - нормальзованная электронная почта
                EmailConfirmed = true,                                      // - электронная почта подтверждена, что такая существует
                PasswordHash = new PasswordHasher<CustomIdentityUser>()     // - пароли базы данных для Администратора
                    .HashPassword(new CustomIdentityUser(), adminName),     // -- 
                SecurityStamp = string.Empty,                               // - 
                PhoneNumberConfirmed = true,                                // - номер телефона подтвержден (хоть и не задан).
            });

        // - определяем Админу соответствующую роль (связываем в БД роль Администратора с конкретным Пользователем)
        builder.Entity<IdentityUserRole<string>>()
            .HasData(new IdentityUserRole<string>()
            {
                UserId = userAdminId,
                RoleId = roleAdminId,
            });
    }
}
