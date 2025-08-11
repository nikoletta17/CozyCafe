using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Models.DTO.Admin;

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
