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
    public class CartService: Service<Cart>, ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository): base(cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            return await _cartRepository.GetByUserIdAsync(userId);
        }
    }
}
