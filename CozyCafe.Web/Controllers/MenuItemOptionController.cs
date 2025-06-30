using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices;
using CozyCafe.Models.Domain;
using CozyCafe.Web.Controllers.Generic_Controller;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Controllers
{
    public class MenuItemOptionController : GenericController<MenuItemOption>
    {
        private readonly IMenuItemOptionService _menuItemOptionService;
        private readonly IMapper _mapper;
        public MenuItemOptionController(IMenuItemOptionService menuItemOptionService, IMapper mapper)
            :base (menuItemOptionService)
        {
            _menuItemOptionService = menuItemOptionService;
            _mapper = mapper;
        }

        public async Task<IActionResult> ByGroup(int  groupId)


        //Task<IEnumerable<MenuItemOption>> GetByGroupIdAsync(int groupId);

    }
}
