using AutoMapper;
using CozyCafe.Application.Exceptions;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Models.Domain.Common;
using CozyCafe.Models.DTO.Admin;

public class AdminOrderService : IAdminOrderService

/// <summary>
/// (UA) Сервіс для роботи з замовленнями у адміністративній частині CozyCafe.  
/// Виконує бізнес-логіку для отримання, фільтрації та оновлення замовлень,  
/// працюючи з репозиторієм і використовуючи AutoMapper для трансформації у DTO.  
/// Основні функції сервісу:
/// - Отримання всіх замовлень з деталями.
/// - Отримання конкретного замовлення за ID.
/// - Оновлення статусу замовлення з перевіркою допустимих значень.
/// - Фільтрація замовлень за статусом та пошук за користувачем.
/// Сервіс також веде централізоване логування всіх операцій через ILoggerService.
///
/// (EN) Service for managing orders in the CozyCafe admin area.  
/// Performs business logic for retrieving, filtering, and updating orders,  
/// working with the repository and using AutoMapper to map to DTOs.  
/// Main functionalities of the service:
/// - Retrieve all orders with details.
/// - Retrieve a specific order by ID.
/// - Update order status with validation of allowed values.
/// - Filter orders by status and search by user.
/// The service also performs centralized logging of all operations via ILoggerService.
/// </summary>
 
{
    private readonly IAdminOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public AdminOrderService(IAdminOrderRepository orderRepository, IMapper mapper, ILoggerService logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<AdminOrderDto>> GetAllOrdersAsync()
    {
        _logger.LogInfo("Отримання всіх замовлень (адмін).");
        var orders = await _orderRepository.GetAllWithDetailsAsync();
        _logger.LogInfo($"Знайдено {orders.Count()} замовлень.");
        return _mapper.Map<List<AdminOrderDto>>(orders);
    }

    public async Task<AdminOrderDto> GetOrderByIdAsync(int id)
    {
        _logger.LogInfo($"Отримання замовлення з ID={id} (адмін).");
        var order = await _orderRepository.GetByIdWithDetailsAsync(id);
        if (order == null)
        {
            _logger.LogWarning($"Замовлення з ID={id} не знайдено.");
            throw new OrderItemNotFoundException(id); 
        }
        return _mapper.Map<AdminOrderDto>(order);
    }

    public async Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto)
    {
        _logger.LogInfo($"Оновлення статусу замовлення ID={dto.OrderId} на '{dto.NewStatus}'.");

        var order = await _orderRepository.GetByIdAsync(dto.OrderId);
        if (order == null)
        {
            _logger.LogWarning($"Замовлення ID={dto.OrderId} не знайдено.");
            throw new OrderItemNotFoundException(dto.OrderId);
        }

        if (!Enum.TryParse<Order.OrderStatus>(dto.NewStatus, true, out var newStatus))
        {
            _logger.LogWarning($"Невідомий статус '{dto.NewStatus}' для замовлення ID={dto.OrderId}.");
            throw new InvalidOrderStatusException(order.Status.ToString(), dto.NewStatus);
        }

        order.Status = newStatus;
        _orderRepository.Update(order);
        await _orderRepository.SaveChangesAsync();

        _logger.LogInfo($"Статус замовлення ID={dto.OrderId} успішно змінено на '{dto.NewStatus}'.");
        return true;
    }

    public async Task<List<AdminOrderDto>> GetOrdersFilteredAsync(string? statusFilter, string? userSearch)
    {
        _logger.LogInfo($"Фільтрація замовлень: статус='{statusFilter}', користувач='{userSearch}'.");
        var orders = await _orderRepository.GetFilteredAsync(statusFilter, userSearch);
        _logger.LogInfo($"Після фільтрації знайдено {orders.Count()} замовлень.");
        return _mapper.Map<List<AdminOrderDto>>(orders);
    }
}
