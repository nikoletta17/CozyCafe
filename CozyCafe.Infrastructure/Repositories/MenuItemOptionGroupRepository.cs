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
    public class MenuItemOptionGroupRepository : Repository<MenuItemOptionGroup>, IMenuItemOptionGroupRepository
    {
        public MenuItemOptionGroupRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MenuItemOptionGroup>> GetAllWithOptionsAsync()
        {
            return await _dbSet
                .Include(g => g.Options)
                .ToListAsync();

        }
    }
}
