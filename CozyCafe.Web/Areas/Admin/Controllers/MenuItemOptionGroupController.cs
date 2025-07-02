using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Services;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.DTO;
using CozyCafe.Web.Areas.User.Controllers.Generic_Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class MenuItemOptionGroupController : GenericController<MenuItemOptionGroup>
    {
        private readonly IMenuItemOptionGroupService _menuItemOptionGroupService;
        private readonly IMapper _mapper;
        public MenuItemOptionGroupController(IMenuItemOptionGroupService menuItemOptionGroupService, IMapper mapper)
            : base(menuItemOptionGroupService)
        {
            _menuItemOptionGroupService = menuItemOptionGroupService;
            _mapper = mapper;
        }

        // GET: MenuItemOptionGroup/WithOptions
        public async Task<IActionResult> WithOptions()
        {
            var groups = await _menuItemOptionGroupService.GetAllWithOptionsAsync();
            var dtos = _mapper.Map<IEnumerable<MenuItemOptionGroup>>(groups);
            return View("Index", dtos);
        }

        // GET: MenuItemOptionGroup/ByMenuItem/id
        public async Task<IActionResult> ByMenuItem(int menuItemId)
        {
            var groups = await _menuItemOptionGroupService.GetByMenuItemIdAsync(menuItemId);
            var dtos = _mapper.Map<IEnumerable<MenuItemOptionGroup>>(groups);
            return View("Index", dtos);
        }
    }
}