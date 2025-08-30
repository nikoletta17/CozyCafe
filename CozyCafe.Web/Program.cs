using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Infrastructure.Services.Logging;
using CozyCafe.Web.Middleware;
using System.Globalization;
using Serilog;
using CozyCafe.Application.Services.ForAdmin;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Додаємо DbContext із SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//Currency
#region Currency
var cultureInfo = new CultureInfo("uk-UA");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
#endregion Currency

//Кешування
builder.Services.AddMemoryCache();

//Cookies
#region Cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    // Кука доступна лише по HTTP (не через JS)
    options.Cookie.HttpOnly = true;

    // Кука передається тільки по HTTPS (для продакшену)
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    // Захист від CSRF: кука не відправляється на сторонні сайти
    options.Cookie.SameSite = SameSiteMode.Lax;

    // Ім'я куки (можеш змінити, щоб було унікальним)
    options.Cookie.Name = "CozyCafeAuthCookie";

    // Термін життя куки для "Запомнить меня" — 14 днів
    options.ExpireTimeSpan = TimeSpan.FromDays(14);

    // Оновлюємо термін дії куки при активності користувача
    options.SlidingExpiration = true;

    // Шляхи для перенаправлення (підкоригуй під свої)
    options.LoginPath = "/User/Account/Login";
    options.LogoutPath = "/User/Account/Logout";

});
#endregion

// Реєстрація репозиторіїв
#region Repository DI
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IMenuItemOptionGroupRepository, MenuItemOptionGroupRepository>();
builder.Services.AddScoped<IMenuItemOptionRepository, MenuItemOptionRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IAdminOrderRepository, AdminOrderRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
#endregion

// Реєстрація сервісів
#region Service DI
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IMenuItemOptionGroupService, MenuItemOptionGroupService>();
builder.Services.AddScoped<IMenuItemOptionService, MenuItemOptionService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAdminOrderService, AdminOrderService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoggerService, LoggerService>();
#endregion

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Exception filter для девелоперів
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Сесії
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//Налаштування ідентифікації та ролей
#region Identity and Roles Configuration
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Ось сюди вставляємо налаштування кукі
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Account/Login";          // Твій кастомний шлях логіну
    options.AccessDeniedPath = "/User/Account/AccessDenied"; // (опціонально) сторінка "Доступ заборонено"
});
#endregion Identity and Roles Configuration

// Додаємо MVC контролери з Views
builder.Services.AddControllersWithViews();

// Логування в консоль
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // лог у файл за днями
    .CreateLogger();

builder.Host.UseSerilog(logger); 
var app = builder.Build();

// Обробка помилок
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // Для всіх необроблених винятків (500)
    app.UseExceptionHandler("/Error");

    // Для всіх кодів помилок (404, 403, 401 і т.д.)
    app.UseStatusCodePagesWithReExecute("/Error/{0}");

    app.UseHsts();
}

// Перехоплення помилок
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode >= 400 && !context.Response.HasStarted)
    {
        var statusCode = context.Response.StatusCode;
        context.Request.Path = $"/Error/{statusCode}";
        await next();
    }
});


// HTTPS і статистика
app.UseHttpsRedirection();
app.UseStaticFiles();

// Маршрутизація
app.UseRouting();

// Сесії
app.UseSession();

// Аутентифікація и авторизація
app.UseAuthentication();
app.UseAuthorization();

// Маршрути Areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// Основний маршрут
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Razor Pages (Identity UI)
app.MapRazorPages();

// Ініціалізація ролей
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // Seed ролі та адміна
    await DbInitializer.SeedRolesAsync(services);
    await DbInitializer.SeedAdminAsync(services);

    //Seed дані (категорії, страви, опції)
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate(); 
}

app.Run();