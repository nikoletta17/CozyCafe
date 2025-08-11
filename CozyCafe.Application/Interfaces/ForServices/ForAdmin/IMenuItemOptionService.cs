using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain.Admin;

namespace CozyCafe.Application.Interfaces.ForServices.ForAdmin
{
    public interface IMenuItemOptionService: IService<MenuItemOption>
    {
        Task<IEnumerable<MenuItemOption>> GetByGroupIdAsync(int groupId);
        Task<IEnumerable<MenuItemOption>> GetByIdsAsync(IEnumerable<int> ids);

    }
}
