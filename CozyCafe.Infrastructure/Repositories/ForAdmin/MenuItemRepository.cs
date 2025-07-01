using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Infrastructure.Data;
using CozyCafe.Infrastructure.Repositories.Generic_Repository;
using CozyCafe.Models.Domain.Admin;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CozyCafe.Infrastructure.Repositories.ForAdmin
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<MenuItem>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(m => m.Category)
                .Where(m => m.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MenuItem>> SearchAsync(string keyword)
        {
            return await _dbSet
                .Include(m => m.Category)
                .Where(m => m.Name.Contains(keyword) || m.Description != null && m.Description.Contains(keyword))
                .ToListAsync();
        }
    }
}
