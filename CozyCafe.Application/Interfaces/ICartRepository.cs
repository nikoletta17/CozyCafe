using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Interfaces
{
    public interface ICartRepository: IRepository<Cart>
    {
        Task<Cart?> GetByUserIdAsync(string userId);
        Task AddOrUpdateCartItemAsync(string userId, CartItem newItem);
        Task RemoveCartItemAsync(string userId, int menuItemId);
        Task ClearCartAsync(string userId);
    }
}
