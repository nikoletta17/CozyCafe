using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository;
using CozyCafe.Application.Interfaces.ForServices;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CozyCafe.Application.Services
{
    public class MenuItemOptionService: Service<MenuItemOption>, IMenuItemOptionService
    {
        private readonly IMenuItemOptionRepository _menuItemOptionRepository;
        public MenuItemOptionService(IMenuItemOptionRepository menuItemOptionRepository): base(menuItemOptionRepository) 
        {
            _menuItemOptionRepository = menuItemOptionRepository;
        }

        public async Task<IEnumerable<MenuItemOption>> GetByGroupIdAsync(int groupId)
        {
            return await _menuItemOptionRepository.GetByGroupIdAsync(groupId);
        }

    }
}
