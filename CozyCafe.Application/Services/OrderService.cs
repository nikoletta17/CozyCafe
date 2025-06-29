using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository;
using CozyCafe.Application.Interfaces.ForServices;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Services
{

    public class OrderService : Service<Order>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
            : base(orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
        {
            return await _orderRepository.GetByUserIdAsync(userId);
        }

        public async Task<Order?> GetFullOrderAsync(int orderId)
        {
            return await _orderRepository.GetFullOrderAsync(orderId);
        }

        public async Task AddOrderItemAsync(int orderId, OrderItem item)
        {
            var order = await _orderRepository.GetFullOrderAsync(orderId);
            if (order == null)
                throw new Exception("Order not found");

            order.Items.Add(item);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task AddOptionToOrderItemAsync(int orderId, int orderItemId, OrderItemOption option)
        {
            var order = await _orderRepository.GetFullOrderAsync(orderId);
            if (order == null)
                throw new Exception("Order not found");

            var orderItem = order.Items.FirstOrDefault(i => i.Id == orderItemId);
            if (orderItem == null)
                throw new Exception("Order item not found");

            orderItem.SelectedOptions.Add(option);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task RemoveOrderItemAsync(int orderId, int orderItemId)
        {
            var order = await _orderRepository.GetFullOrderAsync(orderId);
            if (order == null) return;

            var item = order.Items.FirstOrDefault(i => i.Id == orderItemId);
            if (item != null)
            {
                order.Items.Remove(item);
                await _orderRepository.SaveChangesAsync();
            }
        }

        public async Task RemoveOrderItemOptionAsync(int orderId, int orderItemId, int optionId)
        {
            var order = await _orderRepository.GetFullOrderAsync(orderId);
            if (order == null) return;

            var orderItem = order.Items.FirstOrDefault(i => i.Id == orderItemId);
            if (orderItem == null) return;

            var option = orderItem.SelectedOptions.FirstOrDefault(o => o.Id == optionId);
            if (option != null)
            {
                orderItem.SelectedOptions.Remove(option);
                await _orderRepository.SaveChangesAsync();
            }
        }
    }


}

