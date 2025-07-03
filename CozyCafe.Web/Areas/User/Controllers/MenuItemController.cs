using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
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
            // Якщо користувач ввів назву категорії - знаходимо її Id для подальшої фільтрації
            if (!string.IsNullOrWhiteSpace(filter.CategoryName))
            {
                var allCategories = await _categoryService.GetAllAsync();
                var matchedCategory = allCategories.FirstOrDefault(c => c.Name == filter.CategoryName);
                if (matchedCategory != null)
                {
                    filter.CategoryId = matchedCategory.Id;
                }
                else
                {
                    // Якщо категорія з такою назвою не знайдена - очистити CategoryId
                    filter.CategoryId = null;
                }
            }

            // Отримуємо відфільтровані меню-елементи
            var filteredItems = await _menuItemService.GetFilteredAsync(filter);

            // Для списку фільтрації категорій завантажуємо всі категорії
            var categories = await _categoryService.GetAllAsync();

            // Передаємо категорії для фільтру у ViewBag як SelectList (Id - значення, Name - текст)
            ViewBag.Categories = new SelectList(categories, "Id", "Name", filter.CategoryId);

            // Опції сортування для випадаючого списку
            var sortOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "За замовчуванням" },
                new SelectListItem { Value = "name", Text = "Назва" },
                new SelectListItem { Value = "price_asc", Text = "Ціна ↑" },
                new SelectListItem { Value = "price_desc", Text = "Ціна ↓" },
            };

            // Встановлюємо вибрану опцію сортування
            foreach (var option in sortOptions)
            {
                option.Selected = option.Value == filter.SortBy;
            }

            ViewBag.SortOptions = sortOptions;

            // Передаємо у View кортеж: список меню і модель фільтрації
            return View((filteredItems, filter));
        }
    }
}
