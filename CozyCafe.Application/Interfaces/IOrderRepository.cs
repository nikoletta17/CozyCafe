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
        Task AddOrderItemAsync(int orderId, OrderItem item);
        Task AddOptionToOrderItemAsync(int orderId, int orderItemId, OrderItemOption option);

        Task RemoveOrderItemAsync(int orderId, int orderItemId);
        Task RemoveOrderItemOptionAsync(int orderItemId, int optionId);

    }
}
