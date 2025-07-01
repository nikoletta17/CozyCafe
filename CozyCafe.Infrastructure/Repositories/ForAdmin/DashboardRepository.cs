using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Infrastructure.Data;
using CozyCafe.Models.DTO.Admin;
using Microsoft.EntityFrameworkCore;

namespace CozyCafe.Infrastructure.Repositories.ForAdmin
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Orders.SumAsync(o => o.Total);
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<IEnumerable<TopMenuItemDto>> GetTopMenuItemsAsync(int topCount)
        {
            return await _context.OrderItems
                .GroupBy(oi => oi.MenuItem!.Name)
                .Select(g => new TopMenuItemDto
                {
                    Name = g.Key,
                    QuantitySold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(topCount)
                .ToListAsync();
        }
    }
}
