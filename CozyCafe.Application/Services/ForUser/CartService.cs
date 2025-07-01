using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForUser;
using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.ForUser;

namespace CozyCafe.Application.Services.ForUser
{
    public class CartService : Service<Cart>, ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
            : base(cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            return await _cartRepository.GetByUserIdAsync(userId);
        }

        public async Task AddOrUpdateCartItemAsync(string userId, CartItem newItem)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId, Items = new List<CartItem>() };
                await _cartRepository.AddAsync(cart); 
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.MenuItemId == newItem.MenuItemId);
            if (existingItem != null)
            {
                existingItem.Quantity += newItem.Quantity;
            }
            else
            {
                cart.Items.Add(newItem);
            }

            await _cartRepository.SaveChangesAsync(); 
        }

        public async Task UpdateItemQuantityAsync(string userId, int menuItemId, int quantity)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null) return;

            var item = cart.Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (item != null)
            {
                if (quantity <= 0)
                    cart.Items.Remove(item);  // Якщо кількість 0 або менше — видаляємо
                else
                    item.Quantity = quantity;  // Інакше оновлюємо кількість

                await _cartRepository.SaveChangesAsync();
            }
        }

        public async Task RemoveCartItemAsync(string userId, int menuItemId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null) return;

            var item = cart.Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (item != null)
            {
                cart.Items.Remove(item);
                await _cartRepository.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null) return;

            cart.Items.Clear();
            await _cartRepository.SaveChangesAsync();
        }
    }


}
