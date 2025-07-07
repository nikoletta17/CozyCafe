using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain.Common;

namespace CozyCafe.Application.Interfaces.ForServices.ForUser
{
    public interface IOrderService : IService<Order>
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
        Task<Order?> GetFullOrderAsync(int orderId);
        Task RemoveOrderItemAsync(int orderId, int orderItemId);
        Task AddOrderItemAsync(int orderId, OrderItem item);
        Task UpdateOrderStatusAsync(int orderId, string newStatus);
        Task AddOptionToOrderItemAsync(int orderId, int orderItemId, OrderItemOption option);
        Task RemoveOrderItemOptionAsync(int orderId, int orderItemId, int optionId);
    }

}
