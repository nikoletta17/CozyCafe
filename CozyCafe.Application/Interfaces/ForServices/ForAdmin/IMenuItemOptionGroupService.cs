using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain.Admin;

namespace CozyCafe.Application.Interfaces.ForServices.ForAdmin
{
    public interface IMenuItemOptionGroupService: IService<MenuItemOptionGroup>
    {
        Task<IEnumerable<MenuItemOptionGroup>> GetAllWithOptionsAsync();
        Task<IEnumerable<MenuItemOptionGroup>> GetByMenuItemIdAsync(int menuItemId);
    }
}
