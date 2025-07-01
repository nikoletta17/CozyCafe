using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Interfaces.ForRerository.ForAdmin
{
    public interface IDiscountRepository: IRepository<Discount>
    {
        Task<Discount?> GetByCodeAsync(string code);
    }
}
