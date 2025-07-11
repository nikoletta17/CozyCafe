using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Models.DTO.Admin;

namespace CozyCafe.Application.Services.ForAdmin
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            var totalOrders = await _dashboardRepository.GetTotalOrdersAsync();
            var totalRevenue = await _dashboardRepository.GetTotalRevenueAsync();
            var totalUsers = await _dashboardRepository.GetTotalUsersAsync();
            var topMenuItemsEnumerable = await _dashboardRepository.GetTopMenuItemsAsync(5);

            return new DashboardStatsDto
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                TotalUsers = totalUsers,
                TopMenuItems = topMenuItemsEnumerable.ToList() // Перетворення в List
            };
        }
    }
}
