using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository;
using CozyCafe.Application.Interfaces.ForServices;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Services
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
