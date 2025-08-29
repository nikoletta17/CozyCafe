using CozyCafe.Application.Exceptions;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Models.Domain.Common;
using CozyCafe.Models.DTO.Admin;
using Microsoft.Extensions.Logging;

/// <summary>
/// (UA) Сервіс для управління статусами замовлень у адміністративній частині CozyCafe.  
/// Основні функції:  
/// - Оновлення статусу замовлення за допомогою DTO `UpdateOrderStatusDto`.  
/// - Логування всіх операцій для контролю змін статусів.  
/// - Викидає `OrderItemNotFoundException` якщо замовлення не знайдено.  
/// - Викидає `InvalidOrderStatusException` якщо новий статус некоректний.
///
/// (EN) Service for managing order statuses in the CozyCafe admin area.  
/// Main functionalities:  
/// - Update order status using `UpdateOrderStatusDto`.  
/// - Logs all operations to monitor status changes.  
/// - Throws `OrderItemNotFoundException` if the order is not found.  
/// - Throws `InvalidOrderStatusException` if the new status is invalid.
/// </summary>
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
            throw new OrderItemNotFoundException(dto.OrderId);
        }

        if (!Enum.TryParse<Order.OrderStatus>(dto.NewStatus, true, out var newStatus))
        {
            _logger.LogWarning("Invalid status '{Status}' provided for OrderId={OrderId}", dto.NewStatus, dto.OrderId);
            throw new InvalidOrderStatusException(order.Status.ToString(), dto.NewStatus);
        }

        order.Status = newStatus;
        await _orderStatusRepository.SaveChangesAsync();

        _logger.LogInformation("Order status updated successfully for OrderId={OrderId}", dto.OrderId);
        return true;
    }
}
