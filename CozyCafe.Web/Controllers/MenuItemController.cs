using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.DTO.Admin;
using CozyCafe.Web.Controllers.Generic_Controller;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Controllers
{
    public class MenuItemController : GenericController<MenuItem>
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IMapper _mapper;

        public MenuItemController(IMenuItemService service, IMapper mapper)
            : base(service)
        {
            _menuItemService = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> ByCategory(int categoryId)
        {
            var items = await _menuItemService.GetByCategoryAsync(categoryId);
            var dtos = _mapper.Map<IEnumerable<MenuItemDto>>(items);
            return View("Index", dtos);
        }

        public async Task<IActionResult> Search(string keyword)
        {
            var items = await _menuItemService.SearchAsync(keyword);
            var dtos = _mapper.Map<IEnumerable<MenuItemDto>>(items);
            return View("Index", dtos);
        }
    }
}
