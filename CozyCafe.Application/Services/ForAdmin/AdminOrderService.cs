using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Models.Domain.Common;
using CozyCafe.Models.DTO.Admin;

namespace CozyCafe.Application.Services.ForAdmin
{
    public class AdminOrderService : IAdminOrderService
    {
        private readonly IAdminOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public AdminOrderService(IAdminOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<List<AdminOrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllWithDetailsAsync();
            return _mapper.Map<List<AdminOrderDto>>(orders);
        }

        public async Task<AdminOrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdWithDetailsAsync(id);
            return order == null ? null : _mapper.Map<AdminOrderDto>(order);
        }

        public async Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(dto.OrderId);
            if (order == null)
                return false;

            if (!Enum.TryParse<Order.OrderStatus>(dto.NewStatus, true, out var newStatus))
                return false;

            order.Status = newStatus;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return true;
        }

        public async Task<List<AdminOrderDto>> GetOrdersFilteredAsync(string? statusFilter, string? userSearch)
        {
            var orders = await _orderRepository.GetFilteredAsync(statusFilter, userSearch);
            return _mapper.Map<List<AdminOrderDto>>(orders);
        }
    }
}
