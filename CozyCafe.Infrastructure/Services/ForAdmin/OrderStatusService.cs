using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Models.Domain.Common;
using CozyCafe.Models.DTO.Admin;
using Microsoft.Extensions.Logging;

public class OrderStatusService : IOrderStatusService
{
    private readonly IOrderStatusRepository _orderStatusRepository;
    private readonly ILogger<OrderStatusService> _logger;

    public OrderStatusService(IOrderStatusRepository orderStatusRepository,
                              ILogger<OrderStatusService> logger)
    {
        _orderStatusRepository = orderStatusRepository;
        _logger = logger;
    }

    public async Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto)
    {
        _logger.LogInformation("Updating order status. OrderId={OrderId}, NewStatus={NewStatus}", dto.OrderId, dto.NewStatus);

        var order = await _orderStatusRepository.GetOrderByIdAsync(dto.OrderId);
        if (order == null)
        {
            _logger.LogWarning("Order with Id={OrderId} not found", dto.OrderId);
            return false;
        }

        if (!Enum.TryParse<Order.OrderStatus>(dto.NewStatus, true, out var newStatus))
        {
            _logger.LogWarning("Invalid status '{Status}' provided for OrderId={OrderId}", dto.NewStatus, dto.OrderId);
            return false;
        }

        order.Status = newStatus;
        await _orderStatusRepository.SaveChangesAsync();

        _logger.LogInformation("Order status updated successfully for OrderId={OrderId}", dto.OrderId);
        return true;
    }
}
