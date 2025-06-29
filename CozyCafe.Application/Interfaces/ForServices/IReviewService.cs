using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Interfaces.ForServices
{
    public interface IReviewService: IService<Review>
    {
        Task<IEnumerable<Review>> GetByMenuItemIdAsync(int menuItemId);
        Task<IEnumerable<Review>> GetByUserIdAsync(string userId);
    }
}
