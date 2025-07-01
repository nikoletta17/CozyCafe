using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CozyCafe.Application.Interfaces.ForRerository.ForAdmin
{
    public interface IMenuItemRepository : IRepository<MenuItem>
    {
        Task<IEnumerable<MenuItem>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<MenuItem>> SearchAsync(string keyword);

    }
}

