using System;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Services.ForAdmin
{
    public class DiscountService : Service<Discount>, IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
            : base(discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<Discount?> GetByCodeAsync(string code)
        {
            return await _discountRepository.GetByCodeAsync(code);
        }

        public async Task AddDiscountAsync(Discount discount)
        {
            ValidateDiscountDates(discount);
            await _discountRepository.AddAsync(discount);
            await _discountRepository.SaveChangesAsync();
        }

        public async Task UpdateDiscountAsync(Discount discount)
        {
            ValidateDiscountDates(discount);
             _discountRepository.Update(discount);
            await _discountRepository.SaveChangesAsync();
        }

        private void ValidateDiscountDates(Discount discount)
        {
            if (discount.ValidFrom >= discount.ValidTo)
            {
                throw new ArgumentException("ValidTo must be later than ValidFrom");
            }
        }
    }
}
