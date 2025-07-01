using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Infrastructure.Data;
using CozyCafe.Models.Domain.Common;

namespace CozyCafe.Infrastructure.Repositories.ForAdmin
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
