using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.DTO.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CozyCafe.Application.Interfaces.ForRerository.ForAdmin
{
    public interface IMenuItemRepository : IRepository<MenuItem>
    {
        Task<IEnumerable<MenuItem>> GetFilteredAsync(MenuItemFilterModel filter);

    }
}

