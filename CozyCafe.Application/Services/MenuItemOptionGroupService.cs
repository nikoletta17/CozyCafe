using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository;
using CozyCafe.Application.Interfaces.ForServices;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Services
{
    public class MenuItemOptionGroupService: Service<MenuItemOptionGroup>, IMenuItemOptionGroupService
    {
        private readonly IMenuItemOptionGroupRepository _menuItemOptionGroupRepository;
        public MenuItemOptionGroupService(IMenuItemOptionGroupRepository menuItemOptionGroupRepository): base(menuItemOptionGroupRepository) 
        {
            _menuItemOptionGroupRepository = menuItemOptionGroupRepository;
        }

        public async Task<IEnumerable<MenuItemOptionGroup>> GetAllWithOptionsAsync()
        {
            return await _menuItemOptionGroupRepository.GetAllWithOptionsAsync();
        }

        public async Task<IEnumerable<MenuItemOptionGroup>> GetByMenuItemIdAsync(int menuItemId)
        {
            return await _menuItemOptionGroupRepository.GetByMenuItemIdAsync(menuItemId);
        }
    }
}
