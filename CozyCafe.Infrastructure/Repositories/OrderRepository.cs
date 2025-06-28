using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository;
using CozyCafe.Infrastructure.Data;
using CozyCafe.Infrastructure.Repositories.Generic_Repository;
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

        public async Task AddOrderItemAsync(int orderId, OrderItem item)
        {
            var order = await _dbSet
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found");

            order.Items.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task AddOptionToOrderItemAsync(int orderId, int orderItemId, OrderItemOption option)
        {
            var order = await _dbSet
                .Include(o => o.Items)
                    .ThenInclude(i => i.SelectedOptions)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found");

            var orderItem = order.Items.FirstOrDefault(i => i.Id == orderItemId);
            if (orderItem == null)
                throw new Exception("Order item not found");

            orderItem.SelectedOptions.Add(option);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveOrderItemAsync(int orderId, int orderItemId)
        {
            var order = await _dbSet
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return;

            var item = order.Items.FirstOrDefault(i => i.Id == orderItemId);
            if (item != null)
            {
                order.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveOrderItemOptionAsync(int orderItemId, int optionId)
        {
            var orderItem = await _context.Set<OrderItem>()
                .Include(i => i.SelectedOptions)
                .FirstOrDefaultAsync(i => i.Id == orderItemId);

            if (orderItem == null)
                return;

            var option = orderItem.SelectedOptions.FirstOrDefault(o => o.Id == optionId);
            if (option != null)
            {
                orderItem.SelectedOptions.Remove(option);
                await _context.SaveChangesAsync();
            }
        }

    }
}
