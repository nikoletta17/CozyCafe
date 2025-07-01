using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Infrastructure.Data;
using CozyCafe.Infrastructure.Repositories.Generic_Repository;
using CozyCafe.Models.Domain.Admin;
using Microsoft.EntityFrameworkCore;


namespace CozyCafe.Infrastructure.Repositories.ForAdmin
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
