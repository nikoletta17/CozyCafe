using AutoMapper;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Models.Domain.Common;
using CozyCafe.Models.DTO.Admin;

public class AdminOrderService : IAdminOrderService
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

    public async Task<AdminOrderDto?> GetOrderByIdAsync(int id)
    {
        _logger.LogInfo($"Отримання замовлення з ID={id} (адмін).");
        var order = await _orderRepository.GetByIdWithDetailsAsync(id);
        if (order == null)
        {
            _logger.LogWarning($"Замовлення з ID={id} не знайдено.");
            return null;
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
            return false;
        }

        if (!Enum.TryParse<Order.OrderStatus>(dto.NewStatus, true, out var newStatus))
        {
            _logger.LogWarning($"Невідомий статус '{dto.NewStatus}' для замовлення ID={dto.OrderId}.");
            return false;
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
