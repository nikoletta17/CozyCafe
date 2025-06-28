using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces;
using CozyCafe.Infrastructure.Data;
using CozyCafe.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CozyCafe.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
        {
            return await _dbSet
               .Include(o => o.User)
               .Include(o => o.Discount)
               .Include(o => o.Items)                
                   .ThenInclude(i => i.MenuItem)    
               .Where(o => o.UserId == userId)
               .ToListAsync();
        }
        
        public async Task<Order?> GetFullOrderAsync(int orderId)
        {
            return await _dbSet
                .Include(o => o.Discount)
                .Include(o => o.User)
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}
