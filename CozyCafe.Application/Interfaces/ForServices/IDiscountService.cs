using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Interfaces.ForServices
{
    public interface IDiscountService: IService<Discount>
    {
        Task<Discount?> GetByCodeAsync(string code);
    }
}
