using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain.ForUser;

namespace CozyCafe.Application.Interfaces.ForRerository.ForUser
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetByMenuItemIdAsync(int menuItemId);
        Task<IEnumerable<Review>> GetByUserIdAsync(string userId);
    }
}
