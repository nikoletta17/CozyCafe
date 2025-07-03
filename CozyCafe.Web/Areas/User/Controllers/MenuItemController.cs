using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.DTO.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CozyCafe.Web.Areas.User.Controllers
{
    [Area("User")]
    [Route("User/[controller]/[action]")]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        private readonly ICategoryService _categoryService;

        public MenuItemController(IMenuItemService menuItemService, ICategoryService categoryService)
        {
            _menuItemService = menuItemService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(MenuItemFilterModel filter)
        {
            var allCategories = await _categoryService.GetAllAsync();

            // Якщо прийшла назва категорії, підставити її в CategoryId для фільтрації та селекту
            if (!string.IsNullOrEmpty(filter.CategoryName) && !filter.CategoryId.HasValue)
            {
                var cat = allCategories.FirstOrDefault(c => c.Name == filter.CategoryName);
                if (cat != null)
                {
                    filter.CategoryId = cat.Id;
                }
            }

            var items = await _menuItemService.GetFilteredAsync(filter);

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
    }
}
