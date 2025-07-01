using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Infrastructure.Data;
using CozyCafe.Infrastructure.Repositories.Generic_Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CozyCafe.Application.Mapping;
using CozyCafe.Application.Services.Initialization;
using CozyCafe.Models.Domain.ForUser;
using CozyCafe.Application.Services.ForAdmin;
using CozyCafe.Application.Services.ForUser;
using CozyCafe.Application.Interfaces.ForRerository.ForUser;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Infrastructure.Repositories.ForAdmin;
using CozyCafe.Infrastructure.Repositories.ForUser;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Додаємо DbContext із SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Реєстрація репозиторіїв
#region Repository DI
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
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
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IMenuItemOptionGroupService, MenuItemOptionGroupService>();
builder.Services.AddScoped<IMenuItemOptionService, MenuItemOptionService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAdminOrderService, AdminOrderService>();

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
#endregion

// Додаємо MVC контролери з Views
builder.Services.AddControllersWithViews();

// Логування в консоль
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Міграції або обробка помилок
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();  // додаємо аутентифікацію
app.UseAuthorization();   // додаємо авторизацію

// Маршрутизація MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Маршрути Razor Pages (для Identity UI)
app.MapRazorPages();

// Ініціалізація ролей
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRolesAsync(services);
    await DbInitializer.SeedAdminAsync(services);
}

app.Run();
