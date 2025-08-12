using CozyCafe.Application.Exceptions;
using CozyCafe.Application.Interfaces.ForRerository.ForUser;
using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.Common;
using Microsoft.Extensions.Logging;
using static CozyCafe.Models.Domain.Common.Order;

public class OrderService : Service<Order>, IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
        : base(orderRepository)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
    {
        _logger.LogInformation("Отримання замовлень користувача {UserId}", userId);
        var orders = await _orderRepository.GetByUserIdAsync(userId);

        if (!orders.Any())
        {
            _logger.LogWarning("Замовлень для користувача {UserId} не знайдено", userId);
            throw new NotFoundException("Orders for user", userId);
        }

        _logger.LogInformation("Знайдено {Count} замовлень для користувача {UserId}", orders.Count(), userId);
        return orders;
    }

    public async Task<Order> GetFullOrderAsync(int orderId)
    {
        _logger.LogInformation("Отримання повного замовлення за Id={OrderId}", orderId);
        var order = await _orderRepository.GetFullOrderAsync(orderId);

        if (order == null)
        {
            _logger.LogWarning("Замовлення Id={OrderId} не знайдено", orderId);
            throw new NotFoundException("Order", orderId);
        }

        return order;
    }

    public async Task AddOrderItemAsync(int orderId, OrderItem item)
    {
        _logger.LogInformation("Додавання позиції до замовлення Id={OrderId}", orderId);

        var order = await _orderRepository.GetFullOrderAsync(orderId);
        if (order == null)
        {
            _logger.LogError("Замовлення Id={OrderId} не знайдено", orderId);
            throw new NotFoundException("Order", orderId);
        }

        order.Items.Add(item);
        RecalculateTotal(order);

        _orderRepository.Update(order);
        await _orderRepository.SaveChangesAsync();

        _logger.LogInformation("Позицію додано і підсумок перераховано для замовлення Id={OrderId}", orderId);
    }

    public async Task RemoveOrderItemAsync(int orderId, int orderItemId)
    {
        _logger.LogInformation("Видалення позиції {OrderItemId} із замовлення {OrderId}", orderItemId, orderId);

        var order = await _orderRepository.GetFullOrderAsync(orderId);
        if (order == null)
        {
            _logger.LogWarning("Замовлення Id={OrderId} не знайдено", orderId);
            throw new NotFoundException("Order", orderId);
        }

        var item = order.Items.FirstOrDefault(i => i.Id == orderItemId);
        if (item == null)
        {
            _logger.LogWarning("Позиція {OrderItemId} не знайдена у замовленні {OrderId}", orderItemId, orderId);
            throw new OrderItemNotFoundException(orderItemId);
        }

        order.Items.Remove(item);
        RecalculateTotal(order);

        _orderRepository.Update(order);
        await _orderRepository.SaveChangesAsync();

        _logger.LogInformation("Позицію {OrderItemId} видалено з замовлення {OrderId}", orderItemId, orderId);
    }

    public async Task AddOptionToOrderItemAsync(int orderId, int orderItemId, OrderItemOption option)
    {
        _logger.LogInformation("Додавання опції до позиції {OrderItemId} замовлення {OrderId}", orderItemId, orderId);

        var order = await _orderRepository.GetFullOrderAsync(orderId);
        if (order == null)
        {
            _logger.LogError("Замовлення Id={OrderId} не знайдено", orderId);
            throw new NotFoundException("Order", orderId);
        }

        var orderItem = order.Items.FirstOrDefault(i => i.Id == orderItemId);
        if (orderItem == null)
        {
            _logger.LogError("Позиція {OrderItemId} не знайдена у замовленні {OrderId}", orderItemId, orderId);
            throw new OrderItemNotFoundException(orderItemId);
        }

        orderItem.SelectedOptions.Add(option);

        _orderRepository.Update(order);
        await _orderRepository.SaveChangesAsync();

        _logger.LogInformation("Опцію додано до позиції {OrderItemId} замовлення {OrderId}", orderItemId, orderId);
    }

    public async Task RemoveOrderItemOptionAsync(int orderId, int orderItemId, int optionId)
    {
        _logger.LogInformation("Видалення опції {OptionId} із позиції {OrderItemId} замовлення {OrderId}", optionId, orderItemId, orderId);

        var order = await _orderRepository.GetFullOrderAsync(orderId);
        if (order == null)
        {
            _logger.LogWarning("Замовлення Id={OrderId} не знайдено", orderId);
            throw new NotFoundException("Order", orderId);
        }

        var orderItem = order.Items.FirstOrDefault(i => i.Id == orderItemId);
        if (orderItem == null)
        {
            _logger.LogWarning("Позиція {OrderItemId} не знайдена у замовленні {OrderId}", orderItemId, orderId);
            throw new OrderItemNotFoundException(orderItemId);
        }

        var option = orderItem.SelectedOptions.FirstOrDefault(o => o.Id == optionId);
        if (option == null)
        {
            _logger.LogWarning("Опція {OptionId} не знайдена у позиції {OrderItemId} замовлення {OrderId}", optionId, orderItemId, orderId);
            throw new NotFoundException("Order item option", optionId);
        }

        orderItem.SelectedOptions.Remove(option);

        _orderRepository.Update(order);
        await _orderRepository.SaveChangesAsync();

        _logger.LogInformation("Опцію {OptionId} видалено з позиції {OrderItemId} замовлення {OrderId}", optionId, orderItemId, orderId);
    }

    public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
    {
        _logger.LogInformation("Оновлення статусу замовлення {OrderId} на {NewStatus}", orderId, newStatus);

        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            _logger.LogError("Замовлення Id={OrderId} не знайдено", orderId);
            throw new NotFoundException("Order", orderId);
        }

        if (Enum.TryParse<OrderStatus>(newStatus, true, out var status))
        {
            // Приклад перевірки допустимості зміни статусу
            if (order.Status == OrderStatus.Completed && status != OrderStatus.Completed)
            {
                _logger.LogError("Спроба змінити статус з {Current} на {Attempted} для замовлення {OrderId}", order.Status, status, orderId);
                throw new InvalidOrderStatusException(order.Status.ToString(), status.ToString());
            }

            order.Status = status;
        }
        else
        {
            _logger.LogError("Невірний статус {NewStatus} для замовлення {OrderId}", newStatus, orderId);
            throw new InvalidOrderStatusException(order.Status.ToString(), newStatus);
        }

        _orderRepository.Update(order);
        await _orderRepository.SaveChangesAsync();

        _logger.LogInformation("Статус замовлення {OrderId} успішно оновлено на {NewStatus}", orderId, newStatus);
    }

    private void RecalculateTotal(Order order)
    {
        order.Total = order.Items.Sum(i => i.Price);
        _logger.LogInformation("Перерахунок загальної суми замовлення Id={OrderId}, новий Total={Total}", order.Id, order.Total);
    }
}
