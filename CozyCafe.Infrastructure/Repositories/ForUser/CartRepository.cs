using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForUser;
using CozyCafe.Infrastructure.Data;
using CozyCafe.Infrastructure.Repositories.Generic_Repository;
using CozyCafe.Models.Domain.ForUser;
using Microsoft.EntityFrameworkCore;


namespace CozyCafe.Infrastructure.Repositories.ForUser
{
    public class CartRepository: Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context) { }
        

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(c => c.User)
                .Include(c => c.Items)
                    .ThenInclude(i => i.MenuItem)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

    }

}
