using System.Collections.Generic;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.DTO.Admin;

namespace CozyCafe.Application.Interfaces.ForServices.ForAdmin
{
    public interface IMenuItemService : IService<MenuItem>
    {
        Task<IEnumerable<MenuItemDto>> GetFilteredAsync(MenuItemFilterModel filterModel);
    }
}
