using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository;
using CozyCafe.Application.Interfaces.ForServices;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Services
{
    public class MenuItemService: Service<MenuItem>, IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        public MenuItemService(IMenuItemRepository menuItemRepository): base (menuItemRepository) 
        {
            _menuItemRepository = menuItemRepository;
        }
        public async Task<IEnumerable<MenuItem>> GetByCategoryAsync(int categoryId)
        {
            return await _menuItemRepository.GetByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<MenuItem>> SearchAsync(string keyword)
        {
            return await _menuItemRepository.SearchAsync(keyword);
        }
    }
}
