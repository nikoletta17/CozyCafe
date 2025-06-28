using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces;
using CozyCafe.Infrastructure.Data;
using CozyCafe.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CozyCafe.Infrastructure.Repositories
{
    public class CartRepository: Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context) { }
        

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(c => c.User)
                .Include(c => c.Items)
                    .ThenInclude(i => i.MenuItem)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddOrUpdateCartItemAsync(string userId, CartItem newItem)
        {
            var cart = await _dbSet
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId, Items = new List<CartItem>() };
                await _dbSet.AddAsync(cart);
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

            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartItemAsync(string userId, int menuItemId)
        {
            var cart = await _dbSet
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return;

            var item = cart.Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (item != null)
            {
                cart.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await _dbSet
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return;

            cart.Items.Clear();
            await _context.SaveChangesAsync();
        }


    }

}
