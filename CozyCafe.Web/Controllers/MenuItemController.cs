using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Services.ForAdmin;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.DTO.Admin;
using CozyCafe.Web.Controllers.Generic_Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CozyCafe.Web.Controllers
{
    public class MenuItemController : GenericController<MenuItem>
    {
        private readonly IMenuItemService _menuItemService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public MenuItemController(IMenuItemService service, IMapper mapper, ICategoryService categoryService)
            : base(service)
        {
            _categoryService = categoryService;
            _menuItemService = service;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(MenuItemFilterModel filter)
        {
            var items = await _menuItemService.GetFilteredAsync(filter);
            var categories = await _categoryService.GetAllAsync();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View((items, filter));
        }

    }
}
