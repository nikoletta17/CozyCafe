using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain.ForUser;

namespace CozyCafe.Application.Interfaces.ForRerository.ForUser
{
    public interface ICartRepository: IRepository<Cart>
    {
        Task<Cart?> GetByUserIdAsync(string userId);
        IQueryable<Cart> Query();
    }
}
