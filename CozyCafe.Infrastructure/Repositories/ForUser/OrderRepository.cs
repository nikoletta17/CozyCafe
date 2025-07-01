using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForUser;
using CozyCafe.Infrastructure.Data;
using CozyCafe.Infrastructure.Repositories.Generic_Repository;
using CozyCafe.Models.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CozyCafe.Infrastructure.Repositories.ForUser
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
