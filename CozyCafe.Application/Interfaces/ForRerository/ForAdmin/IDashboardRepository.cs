using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Models.DTO.Admin;

namespace CozyCafe.Application.Interfaces.ForRerository.ForAdmin
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalOrdersAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<int> GetTotalUsersAsync();
        Task<IEnumerable<TopMenuItemDto>> GetTopMenuItemsAsync(int topCount);
    }
}
