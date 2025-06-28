using CozyCafe.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CozyCafe.Application.Interfaces
{
    public interface IMenuItemRepository : IRepository<MenuItem>
    {
        Task<IEnumerable<MenuItem>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<MenuItem>> SearchAsync(string keyword);

    }
}

