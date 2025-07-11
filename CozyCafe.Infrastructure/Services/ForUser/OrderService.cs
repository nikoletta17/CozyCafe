﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForUser;
using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.Common;
using static CozyCafe.Models.Domain.Common.Order;

namespace CozyCafe.Application.Services.ForUser
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
            RecalculateTotal(order);

            _orderRepository.Update(order);
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
                RecalculateTotal(order);

                _orderRepository.Update(order);
                await _orderRepository.SaveChangesAsync();
            }
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
            // Можна додати зміну ціни опції в ціну item — якщо вона щось коштує

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
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
                // Якщо опція має вартість, треба відняти її від item.Price або Total

                _orderRepository.Update(order);
                await _orderRepository.SaveChangesAsync();
            }
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new Exception("Order not found");

            if (Enum.TryParse<OrderStatus>(newStatus, true, out var status))
            {
                order.Status = status;
            }
            else
            {
                throw new ArgumentException("Invalid status value.");
            }

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Допоміжний метод для перерахунку Total
        /// </summary>
        private void RecalculateTotal(Order order)
        {
            order.Total = order.Items.Sum(i => i.Price);
        }
    }
}
