using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Interfaces.ForServices
{
    public interface IOrderService : IService<Order>
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
        Task<Order?> GetFullOrderAsync(int orderId);
        Task AddOrderItemAsync(int orderId, OrderItem item);
        Task AddOptionToOrderItemAsync(int orderId, int orderItemId, OrderItemOption option);
        Task RemoveOrderItemOptionAsync(int orderId, int orderItemId, int optionId);
    }

}
