using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Infrastructure.Data;
using CozyCafe.Models.Domain.Common;
using Microsoft.EntityFrameworkCore;


namespace CozyCafe.Infrastructure.Repositories.ForAdmin
{
    public class AdminOrderRepository : IAdminOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .Include(o => o.Items)
                    .ThenInclude(i => i.SelectedOptions)
                        .ThenInclude(opt => opt.MenuItemOption)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .Include(o => o.Items)
                    .ThenInclude(i => i.SelectedOptions)
                        .ThenInclude(opt => opt.MenuItemOption)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetFilteredAsync(string? statusFilter, string? userSearch)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .Include(o => o.Items)
                    .ThenInclude(i => i.SelectedOptions)
                        .ThenInclude(opt => opt.MenuItemOption)
                .AsQueryable();

            if (!string.IsNullOrEmpty(statusFilter))
            {
                if (Enum.TryParse<Order.OrderStatus>(statusFilter, out var status))
                {
                    query = query.Where(o => o.Status == status);
                }
            }

            if (!string.IsNullOrEmpty(userSearch))
            {
                userSearch = userSearch.ToLower();
                query = query.Where(o => o.User.FullName.ToLower().Contains(userSearch)
                                      || o.User.Email.ToLower().Contains(userSearch));
            }

            return await query.ToListAsync();
        }

        // Реалізація IRepository<Order>:
        public async Task<IEnumerable<Order>> GetAllAsync() => await _context.Orders.ToListAsync();

        public async Task<Order?> GetByIdAsync(int id) => await _context.Orders.FindAsync(id);

        public async Task AddAsync(Order entity)
        {
            await _context.Orders.AddAsync(entity);
        }

        public void Update(Order entity)
        {
            _context.Orders.Update(entity);
        }

        public void Delete(Order entity)
        {
            _context.Orders.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
