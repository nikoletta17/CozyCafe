using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Models.DTO.Admin;

namespace CozyCafe.Application.Interfaces.ForServices.ForAdmin
{
    public interface IAdminOrderService
    {
        Task<List<AdminOrderDto>> GetAllOrdersAsync();
        Task<AdminOrderDto?> GetOrderByIdAsync(int id);
        Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto);
        Task<List<AdminOrderDto>> GetOrdersFilteredAsync(string? statusFilter, string? userSearch);

    }
}
