using CozyCafe.Models.Domain.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CozyCafe.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        private readonly ICategoryService _categoryService;

        public MenuItemController(IMenuItemService menuItemService, ICategoryService categoryService)
        {
            _menuItemService = menuItemService;
            _categoryService = categoryService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var items = await _menuItemService.GetFilteredAsync(new MenuItemFilterModel());
            return View(items);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            // отримуємо всі категорії верхнього рівня
            var categories = await _categoryService.GetByParentCategoryIdAsync(null);

            var dto = new MenuItemDto
            {
                Name = string.Empty,             // required
                CategoryName = string.Empty,     // required
                Categories = categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList()
            };

            return View(dto);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetByParentCategoryIdAsync(null);
                dto.Categories = categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList();

                return View(dto);
            }

            // мапування DTO у доменну модель
            var menuItem = new MenuItem
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId
            };

            await _menuItemService.AddAsync(menuItem);

            return RedirectToAction("Index");
        }



        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _menuItemService.GetByIdAsync(id); // MenuItemDto
            if (item == null) return NotFound();

            var categories = await _categoryService.GetByParentCategoryIdAsync(null); // всі категорії верхнього рівня
            item.Categories = categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == item.CategoryId
                })
                .ToList();

            return View(item);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuItemDto dto)
        {
            if (id != dto.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetByParentCategoryIdAsync(null);
                dto.Categories = categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name,
                        Selected = c.Id == dto.CategoryId
                    })
                    .ToList();

                return View(dto);
            }

            // мапування DTO у доменну модель
            var menuItem = new MenuItem
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId
            };

            await _menuItemService.UpdateAsync(menuItem);

            return RedirectToAction("Index");
        }


        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // отримуємо доменну модель
            var item = await _menuItemService.GetByIdAsync(id);
            if (item == null) return NotFound();

            // отримуємо назву категорії через сервіс
            var category = await _categoryService.GetByIdAsync(item.CategoryId);

            // формуємо DTO для View
            var dto = new MenuItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                ImageUrl = item.ImageUrl,
                CategoryId = item.CategoryId,
                CategoryName = category?.Name ?? string.Empty
            };

            return View(dto);
        }


    }
}
