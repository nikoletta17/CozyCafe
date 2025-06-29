using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository;
using CozyCafe.Application.Interfaces.ForServices;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Services
{
    public class DiscountService: Service<Discount>, IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        public DiscountService(IDiscountRepository discountRepository): base(discountRepository) 
        {
            _discountRepository = discountRepository;   
        }

        public async Task<Discount?> GetByCodeAsync(string code)
        {
            return await _discountRepository.GetByCodeAsync(code);
        }
    }
}
