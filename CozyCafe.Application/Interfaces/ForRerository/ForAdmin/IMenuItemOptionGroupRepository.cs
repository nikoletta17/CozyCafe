using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain.Admin;

namespace CozyCafe.Application.Interfaces.ForRerository.ForAdmin
{
    public interface IMenuItemOptionGroupRepository : IRepository<MenuItemOptionGroup>
    {
        Task<IEnumerable<MenuItemOptionGroup>> GetAllWithOptionsAsync();
        Task<IEnumerable<MenuItemOptionGroup>> GetByMenuItemIdAsync(int menuItemId); 
    }

}
