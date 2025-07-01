using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Infrastructure.Data;
using CozyCafe.Infrastructure.Repositories.Generic_Repository;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.DTO.Admin;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CozyCafe.Infrastructure.Repositories.ForAdmin
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<MenuItem>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(m => m.Category)
                .Where(m => m.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MenuItem>> SearchAsync(string keyword)
        {
            return await _dbSet
                .Include(m => m.Category)
                .Where(m => m.Name.Contains(keyword) || m.Description != null && m.Description.Contains(keyword))
                .ToListAsync();
        }

        public async Task<IEnumerable<MenuItem>> GetFilteredAsync(MenuItemFilterModel filterModel)
        {
            var query = _context.MenuItems
                .Include(mi => mi.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterModel.SearchTerm))
                query = query.Where(mi => mi.Name.Contains(filterModel.SearchTerm));

            if (filterModel.CategoryId.HasValue)
                query = query.Where(mi => mi.CategoryId == filterModel.CategoryId.Value);

            if (filterModel.MinPrice.HasValue)
                query = query.Where(mi => mi.Price >= filterModel.MinPrice.Value);

            if (filterModel.MaxPrice.HasValue)
                query = query.Where(mi => mi.Price <= filterModel.MaxPrice.Value);

            query = filterModel.SortBy switch
            {
                "price_asc" => query.OrderBy(mi => mi.Price),
                "price_desc" => query.OrderByDescending(mi => mi.Price),
                "name" => query.OrderBy(mi => mi.Name),
                _ => query.OrderBy(mi => mi.Id)
            };

            return await query.ToListAsync();
        }

    }
}
