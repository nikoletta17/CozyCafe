using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Models.DTO.Admin;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Areas.User.Controllers
{
    [Area("User")]
    [Route("User/[controller]/[action]")]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        private readonly ICategoryService _categoryService;
        private readonly IMenuItemOptionGroupService _optionGroupService;

        public MenuItemController(
            IMenuItemService menuItemService,
            ICategoryService categoryService,
            IMenuItemOptionGroupService optionGroupService)
        {
            _menuItemService = menuItemService;
            _categoryService = categoryService;
            _optionGroupService = optionGroupService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(MenuItemFilterModel filter)
        {
            if (!string.IsNullOrEmpty(filter.CategoryName))
            {
                var categories = await _categoryService.GetAllAsync();
                var category = categories.FirstOrDefault(c => c.Name == filter.CategoryName);
                if (category != null)
                {
                    filter.CategoryId = category.Id;
                }
            }

            var items = await _menuItemService.GetFilteredAsync(filter);
            var allCategories = await _categoryService.GetAllAsync();

            ViewBag.Categories = new SelectList(allCategories, "Id", "Name", filter.CategoryId);

            var sortOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "За замовчуванням" },
                new SelectListItem { Value = "name", Text = "Назва" },
                new SelectListItem { Value = "price_asc", Text = "Ціна ↑" },
                new SelectListItem { Value = "price_desc", Text = "Ціна ↓" },
            };

            foreach (var option in sortOptions)
            {
                option.Selected = option.Value == filter.SortBy;
            }

            ViewBag.SortOptions = sortOptions;

            return View((items, filter));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _menuItemService.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            var groups = await _optionGroupService.GetByMenuItemIdAsync(id);
            var optionGroupDtos = groups.Select(g => new MenuItemOptionGroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Options = g.Options.Select(o => new MenuItemOptionDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    ExtraPrice = o.ExtraPrice
                }).ToList()
            }).ToList();

            ViewBag.OptionGroups = optionGroupDtos;
            return View("Details", item);
        }

    }
}
