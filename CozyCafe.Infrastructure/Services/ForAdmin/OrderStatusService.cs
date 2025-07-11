using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Models.Domain.Common;
using CozyCafe.Models.DTO.Admin;

namespace CozyCafe.Application.Services.ForAdmin
{
    public class OrderStatusService : IOrderStatusService
    {
        private readonly IOrderStatusRepository _orderStatusRepository;

        public OrderStatusService(IOrderStatusRepository orderStatusRepository)
        {
            _orderStatusRepository = orderStatusRepository;
        }

        public async Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto)
        {
            var order = await _orderStatusRepository.GetOrderByIdAsync(dto.OrderId);
            if (order == null) return false;

            if (!Enum.TryParse<Order.OrderStatus>(dto.NewStatus, true, out var newStatus))
                return false;

            order.Status = newStatus;
            await _orderStatusRepository.SaveChangesAsync();

            return true;
        }
    }
}
