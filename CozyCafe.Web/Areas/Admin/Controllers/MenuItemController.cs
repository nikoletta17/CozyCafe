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
        private readonly ILogger<MenuItemController> _logger;

        public MenuItemController(IMenuItemService menuItemService, ICategoryService categoryService, ILogger<MenuItemController> logger)
        {
            _menuItemService = menuItemService;
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var items = await _menuItemService.GetFilteredAsync(new MenuItemFilterModel());
            return View(items);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            // Передаємо список категорій у View через ViewBag
            var categories = await _categoryService.GetByParentCategoryIdAsync(null);
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState невалідний");
                return View(dto);
            }

            var menuItem = new MenuItem
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId
            };

            _logger.LogInformation("Спроба додати новий елемент меню: {@MenuItem}", menuItem);

            await _menuItemService.AddAsync(menuItem);

            _logger.LogInformation("Елемент меню збережений успішно");

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

            // Підтягуємо старий елемент з БД
            var existingItem = await _menuItemService.GetByIdAsync(id);
            if (existingItem == null) return NotFound();

            // Якщо поле ImageUrl порожнє у DTO, залишаємо старе значення
            if (string.IsNullOrEmpty(dto.ImageUrl))
            {
                dto.ImageUrl = existingItem.ImageUrl;
            }

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



        // GET: Admin/MenuItem/Delete/id
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _menuItemService.GetByIdAsync(id);
            if (item == null) return NotFound();

            var category = await _categoryService.GetByIdAsync(item.CategoryId);

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

        // POST: Admin/MenuItem/Delete/id
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _menuItemService.GetByIdAsync(id);
            if (item == null) return NotFound();

            await _menuItemService.DeleteAsync(item.Id);

            return RedirectToAction("Index");
        }

    }
}
