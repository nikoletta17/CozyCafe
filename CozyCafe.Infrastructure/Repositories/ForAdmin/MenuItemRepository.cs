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

        public async Task<IEnumerable<MenuItem>> GetFilteredAsync(MenuItemFilterModel filter)
        {
            var query = _context.MenuItems
                .Include(mi => mi.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(x => x.Name.Contains(filter.SearchTerm));
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == filter.CategoryId);
            }

            if (!string.IsNullOrEmpty(filter.CategoryName))
            {
                query = query.Where(x => x.Category.Name == filter.CategoryName);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(x => x.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(x => x.Price <= filter.MaxPrice.Value);
            }

            // Сортування
            switch (filter.SortBy)
            {
                case "name":
                    query = query.OrderBy(x => x.Name);
                    break;
                case "price_asc":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(x => x.Price);
                    break;
            }

            return await query.ToListAsync();
        }

    }
}
