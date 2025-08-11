using CozyCafe.Application.Interfaces.ForRerository.ForUser;
using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.ForUser;
using CozyCafe.Models.DTO.ForUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace CozyCafe.Application.Services.ForUser
{
    public class CartService : Service<Cart>, ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<CartService> _logger;

        public CartService(ICartRepository cartRepository, ILogger<CartService> logger)
            : base(cartRepository)
        {
            _cartRepository = cartRepository;
            _logger = logger;
        }

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            _logger.LogInformation("Отримання кошика для користувача {UserId}", userId);

            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
                _logger.LogWarning("Кошик користувача {UserId} не знайдено", userId);

            return cart;
        }

        public async Task AddOrUpdateItemAsync(string userId, int menuItemId, int quantity, int[] selectedOptionIds)
        {
            _logger.LogInformation(
                "Додавання/оновлення товару {MenuItemId} (кількість {Quantity}) у кошику користувача {UserId} з опціями: {Options}",
                menuItemId, quantity, userId, string.Join(",", selectedOptionIds));

            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                _logger.LogInformation("Кошик не знайдено, створюю новий для користувача {UserId}", userId);
                cart = new Cart { UserId = userId };
                await _cartRepository.AddAsync(cart);
                await _cartRepository.SaveChangesAsync();
            }

            var existingItem = cart.Items
                .FirstOrDefault(i => i.MenuItemId == menuItemId &&
                    i.SelectedOptions.Select(o => o.MenuItemOptionId).OrderBy(id => id)
                    .SequenceEqual(selectedOptionIds.OrderBy(id => id)));

            if (existingItem != null)
            {
                _logger.LogInformation("Оновлюю кількість товару {MenuItemId} у кошику користувача {UserId}", menuItemId, userId);
                existingItem.Quantity += quantity;
            }
            else
            {
                _logger.LogInformation("Додаю новий товар {MenuItemId} у кошик користувача {UserId}", menuItemId, userId);

                var newItem = new CartItem
                {
                    MenuItemId = menuItemId,
                    Quantity = quantity
                };

                foreach (var optionId in selectedOptionIds)
                {
                    newItem.SelectedOptions.Add(new CartItemOption
                    {
                        MenuItemOptionId = optionId
                    });
                }

                cart.Items.Add(newItem);
            }

            await _cartRepository.SaveChangesAsync();
            _logger.LogInformation("Кошик користувача {UserId} успішно оновлено", userId);
        }

        public async Task AddOrUpdateCartItemAsync(string userId, CartItem newItem)
        {
            _logger.LogInformation("Додавання/оновлення товару {MenuItemId} у кошику користувача {UserId}",
                newItem.MenuItemId, userId);

            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                _logger.LogInformation("Кошик не знайдено, створюю новий для користувача {UserId}", userId);
                cart = new Cart { UserId = userId, Items = new List<CartItem>() };
                await _cartRepository.AddAsync(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i =>
                i.MenuItemId == newItem.MenuItemId &&
                i.SelectedOptions.Select(o => o.MenuItemOptionId).OrderBy(id => id)
                .SequenceEqual(newItem.SelectedOptions.Select(o => o.MenuItemOptionId).OrderBy(id => id))
            );

            if (existingItem != null)
            {
                _logger.LogInformation("Оновлюю кількість існуючого товару {MenuItemId} у кошику користувача {UserId}",
                    newItem.MenuItemId, userId);
                existingItem.Quantity += newItem.Quantity;
            }
            else
            {
                _logger.LogInformation("Додаю новий товар {MenuItemId} у кошик користувача {UserId}",
                    newItem.MenuItemId, userId);
                cart.Items.Add(newItem);
            }

            await _cartRepository.SaveChangesAsync();
            _logger.LogInformation("Кошик користувача {UserId} успішно оновлено", userId);
        }

        public async Task<CartDto> GetCartAsync(string userId)
        {
            _logger.LogInformation("Отримання повного кошика для користувача {UserId}", userId);

            var cart = await _cartRepository.Query()
                .Include(c => c.Items)
                    .ThenInclude(i => i.MenuItem)
                .Include(c => c.Items)
                    .ThenInclude(i => i.SelectedOptions)
                        .ThenInclude(o => o.MenuItemOption)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                _logger.LogWarning("Кошик користувача {UserId} не знайдено", userId);
                return new CartDto { Items = new List<CartItemDto>(), Total = 0m };
            }

            var cartDto = new CartDto
            {
                Items = cart.Items.Select(i =>
                {
                    var unitPrice = i.MenuItem.Price + i.SelectedOptions.Sum(o => o.MenuItemOption!.ExtraPrice ?? 0);

                    return new CartItemDto
                    {
                        MenuItemId = i.MenuItemId,
                        MenuItemName = i.MenuItem!.Name,
                        Quantity = i.Quantity,
                        UnitPrice = unitPrice,
                        Price = unitPrice * i.Quantity,
                        ImageUrl = i.MenuItem.ImageUrl,
                        SelectedOptionNames = i.SelectedOptions.Select(o => o.MenuItemOption!.Name).ToList()
                    };
                }).ToList(),

                Total = cart.Items.Sum(i =>
                    (i.MenuItem.Price + i.SelectedOptions.Sum(o => o.MenuItemOption!.ExtraPrice ?? 0)) * i.Quantity)
            };

            _logger.LogInformation("Кошик користувача {UserId} успішно отримано, {ItemCount} товарів",
                userId, cartDto.Items.Count);

            return cartDto;
        }

        public async Task UpdateItemQuantityAsync(string userId, int menuItemId, int quantity)
        {
            _logger.LogInformation(
                "Оновлення кількості товару {MenuItemId} до {Quantity} у кошику користувача {UserId}",
                menuItemId, quantity, userId);

            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogWarning("Кошик користувача {UserId} не знайдено для оновлення товару", userId);
                return;
            }

            var item = cart.Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    _logger.LogInformation("Видаляю товар {MenuItemId} з кошика користувача {UserId}", menuItemId, userId);
                    cart.Items.Remove(item);
                }
                else
                {
                    _logger.LogInformation("Оновлюю кількість товару {MenuItemId} до {Quantity} для користувача {UserId}",
                        menuItemId, quantity, userId);
                    item.Quantity = quantity;
                }

                await _cartRepository.SaveChangesAsync();
                _logger.LogInformation("Кошик користувача {UserId} оновлено", userId);
            }
        }

        public async Task RemoveCartItemAsync(string userId, int menuItemId)
        {
            _logger.LogInformation("Видалення товару {MenuItemId} з кошика користувача {UserId}", menuItemId, userId);

            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogWarning("Кошик користувача {UserId} не знайдено для видалення товару", userId);
                return;
            }

            var item = cart.Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (item != null)
            {
                cart.Items.Remove(item);
                await _cartRepository.SaveChangesAsync();
                _logger.LogInformation("Товар {MenuItemId} видалено з кошика користувача {UserId}", menuItemId, userId);
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            _logger.LogInformation("Очищення кошика користувача {UserId}", userId);

            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogWarning("Кошик користувача {UserId} не знайдено для очищення", userId);
                return;
            }

            cart.Items.Clear();
            await _cartRepository.SaveChangesAsync();
            _logger.LogInformation("Кошик користувача {UserId} очищено", userId);
        }
    }
}
