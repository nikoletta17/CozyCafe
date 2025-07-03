using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.DTO.Admin;
using Microsoft.EntityFrameworkCore;

namespace CozyCafe.Application.Services.ForAdmin
{
    public class MenuItemService : Service<MenuItem>, IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        public MenuItemService(IMenuItemRepository menuItemRepository) : base(menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public async Task<IEnumerable<MenuItemDto>> GetFilteredAsync(MenuItemFilterModel filterModel)
        {
            var items = await _menuItemRepository.GetFilteredAsync(filterModel);

            return items.Select(mi => new MenuItemDto
            {
                Id = mi.Id,
                Name = mi.Name,
                Description = mi.Description,
                Price = mi.Price,
                ImageUrl = mi.ImageUrl,
                CategoryName = mi.Category?.Name ?? ""
            });
        }


    }
}
