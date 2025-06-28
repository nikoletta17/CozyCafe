using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Interfaces.ForRerository
{
    public interface IMenuItemOptionGroupRepository : IRepository<MenuItemOptionGroup>
    {
        Task<IEnumerable<MenuItemOptionGroup>> GetAllWithOptionsAsync();
    }

}
