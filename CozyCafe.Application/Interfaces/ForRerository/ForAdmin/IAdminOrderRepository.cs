using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain.Common;

namespace CozyCafe.Application.Interfaces.ForRerository.ForAdmin
{
    public interface IAdminOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllWithDetailsAsync();
        Task<Order?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Order>> GetFilteredAsync(string? statusFilter, string? userSearch);
    }
}
