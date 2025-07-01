using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Models.Domain.Common;

namespace CozyCafe.Application.Interfaces.ForRerository.ForAdmin
{
    public interface IOrderStatusRepository
    {
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task SaveChangesAsync();
    }
}
