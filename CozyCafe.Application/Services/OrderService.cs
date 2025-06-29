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
    public class OrderService: Service<Order>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository): base (orderRepository) 
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
    }
}
