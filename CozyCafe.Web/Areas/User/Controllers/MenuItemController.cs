using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Services.ForAdmin;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.DTO.Admin;
using CozyCafe.Web.Areas.User.Controllers.Generic_Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CozyCafe.Web.Areas.User.Controllers
{
    [Area("User")]
    [Route("User/[controller]/[action]")]
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
        [HttpGet]
        public async Task<IActionResult> Index(MenuItemFilterModel filter, string? CategoryName)
        {
            var categories = await _categoryService.GetAllAsync();

            // Якщо прийшов CategoryName, знайдемо відповідну категорію та поставимо її Id у фільтр
            if (!string.IsNullOrEmpty(CategoryName))
            {
                var category = categories.FirstOrDefault(c => c.Name == CategoryName);
                if (category != null)
                {
                    filter.CategoryId = category.Id;
                }
            }

            var items = await _menuItemService.GetFilteredAsync(filter);

            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            var sortOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "За замовчуванням" },
                new SelectListItem { Value = "name", Text = "Назва" },
                new SelectListItem { Value = "price_asc", Text = "Ціна ↑" },
                new SelectListItem { Value = "price_desc", Text = "Ціна ↓" },
            };

            // Позначити вибране значення
            foreach (var option in sortOptions)
            {
                option.Selected = option.Value == filter.SortBy;
            }

            ViewBag.SortOptions = sortOptions;

            return View((items, filter));
        }


    }
}
