using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain.Common;

namespace CozyCafe.Application.Interfaces.ForRerository.ForUser
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
        Task<Order?> GetFullOrderAsync(int orderId);
        
    }
}
