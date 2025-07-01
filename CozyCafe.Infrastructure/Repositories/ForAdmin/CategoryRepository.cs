using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Infrastructure.Data;
using CozyCafe.Infrastructure.Repositories.Generic_Repository;
using CozyCafe.Models.Domain.Admin;
using Microsoft.EntityFrameworkCore;

namespace CozyCafe.Infrastructure.Repositories.ForAdmin
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetByParentCategoryIdAsync(int? parentCategoryId)
        {
            return await _dbSet
                .Where(c => c.ParentCategoryId == parentCategoryId)
                .ToListAsync();
        }
        
    }
}
