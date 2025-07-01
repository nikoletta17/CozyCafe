using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.DTO.Admin;
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
        {
            var options = await _menuItemOptionService.GetByGroupIdAsync(groupId);
            var dtos = _mapper.Map<IEnumerable<MenuItemOptionDto>>(options);
            return View("Index", dtos);
        }

    }
}
