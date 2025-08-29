using CozyCafe.Application.Exceptions;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Models.DTO.Admin;

/// <summary>
/// (UA) Сервіс для отримання статистики адміністративної панелі CozyCafe.  
/// Виконує збір основних показників:  
/// - Загальна кількість замовлень  
/// - Загальний дохід  
/// - Загальна кількість користувачів  
/// - Топ меню-елементів за популярністю  
/// Логування операцій забезпечує контроль процесу отримання даних.
/// У разі відсутності даних викидає NotFoundException.
/// 
/// (EN) Service for retrieving statistics for the CozyCafe admin dashboard.  
/// Collects key metrics:  
/// - Total orders  
/// - Total revenue  
/// - Total users  
/// - Top menu items by popularity  
/// Logs operations to monitor the data retrieval process.  
/// Throws NotFoundException if no data is found.
/// </summary>
public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;
    private readonly ILoggerService _logger;

    public DashboardService(IDashboardRepository dashboardRepository, ILoggerService logger)
    {
        _dashboardRepository = dashboardRepository;
        _logger = logger;
    }

    public async Task<DashboardStatsDto> GetStatsAsync()
    {
        _logger.LogInfo("Отримання статистики для адмінської панелі.");

        var totalOrders = await _dashboardRepository.GetTotalOrdersAsync();
        var totalRevenue = await _dashboardRepository.GetTotalRevenueAsync();
        var totalUsers = await _dashboardRepository.GetTotalUsersAsync();
        var topMenuItemsEnumerable = await _dashboardRepository.GetTopMenuItemsAsync(5);

        if (totalOrders == 0 && totalUsers == 0 && totalRevenue == 0)
        {
            _logger.LogWarning("Дані для адмінської панелі не знайдені.");
            throw new NotFoundException("Dashboard statistics", "all");
        }

        _logger.LogInfo($"Статистика: замовлень={totalOrders}, дохід={totalRevenue}, користувачів={totalUsers}.");

        return new DashboardStatsDto
        {
            TotalOrders = totalOrders,
            TotalRevenue = totalRevenue,
            TotalUsers = totalUsers,
            TopMenuItems = topMenuItemsEnumerable.ToList()
        };
    }
}
