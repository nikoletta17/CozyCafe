using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Infrastructure.Services.Logging;
using CozyCafe.Web.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// ������ DbContext �� SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//���������
builder.Services.AddMemoryCache();

//Cookies
#region Cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    // ���� �������� ���� �� HTTP (�� ����� JS)
    options.Cookie.HttpOnly = true;

    // ���� ���������� ����� �� HTTPS (��� ����������)
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    // ������ �� CSRF: ���� �� ������������� �� ������� �����
    options.Cookie.SameSite = SameSiteMode.Lax;

    // ��'� ���� (����� ������, ��� ���� ���������)
    options.Cookie.Name = "CozyCafeAuthCookie";

    // ����� ����� ���� ��� "��������� ����" � 14 ���
    options.ExpireTimeSpan = TimeSpan.FromDays(14);

    // ��������� ����� 䳿 ���� ��� ��������� �����������
    options.SlidingExpiration = true;

    // ����� ��� ��������������� (��������� �� ���)
    options.LoginPath = "/User/Account/Login";
    options.LogoutPath = "/User/Account/Logout";

});
#endregion


// ��������� ����������
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

// ��������� ������
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

// Exception filter ��� ����������
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ���
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

// ��� ���� ���������� ������������ ���
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Account/Login";          // ��� ��������� ���� �����
    options.AccessDeniedPath = "/User/Account/AccessDenied"; // (�����������) ������� "������ ����������"
});
#endregion

// ������ MVC ���������� � Views
builder.Services.AddControllersWithViews();

// ��������� � �������
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // ��� � ���� �� ����
    .CreateLogger();

builder.Host.UseSerilog(logger); // ������������ Serilog

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// ̳������ ��� ������� �������
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

app.UseAuthentication();  // ������ ��������������
app.UseAuthorization();   // ������ �����������

// ������� ��� Areas (�� ���� ����� ��������� ���������)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



// �������� Razor Pages (��� Identity UI)
app.MapRazorPages();

// ����������� �����
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRolesAsync(services);
    await DbInitializer.SeedAdminAsync(services);
}

app.Run();
