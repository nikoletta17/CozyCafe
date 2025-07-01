using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain.ForUser;

namespace CozyCafe.Application.Interfaces.ForServices.ForUser
{
    public interface ICartService: IService<Cart>
    {
        Task<Cart?> GetByUserIdAsync(string userId);
        Task AddOrUpdateCartItemAsync(string userId, CartItem newItem);
        Task UpdateItemQuantityAsync(string userId, int menuItemId, int quantity);
        Task RemoveCartItemAsync(string userId, int menuItemId);
        Task ClearCartAsync(string userId);
    }
}
