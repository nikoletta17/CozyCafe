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
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        public DiscountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Discount?> GetByCodeAsync(string code)
        {
            return await _dbSet
                .FirstOrDefaultAsync(d => d.Code == code);
        }
    }
}
