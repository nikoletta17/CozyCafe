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
    public class MenuItemOptionRepository : Repository<MenuItemOption>, IMenuItemOptionRepository
    {
        public MenuItemOptionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MenuItemOption>> GetByGroupIdAsync(int groupId)
        {
            return await _dbSet
                 .Where(o => o.OptionGroupId == groupId)
                .ToListAsync();
        }
    }
}
