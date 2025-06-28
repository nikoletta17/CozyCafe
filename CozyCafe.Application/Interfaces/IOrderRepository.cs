using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
        Task<Order?> GetFullOrderAsync(int orderId);

    }
}
