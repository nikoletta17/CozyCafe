using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository;
using CozyCafe.Infrastructure.Data;
using CozyCafe.Infrastructure.Repositories.Generic_Repository;
using CozyCafe.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CozyCafe.Infrastructure.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Review>> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByMenuItemIdAsync(int menuItemId)
        {
            return await _dbSet
                .Include(r => r.MenuItem)
                .Where(r => r.MenuItemId == menuItemId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
