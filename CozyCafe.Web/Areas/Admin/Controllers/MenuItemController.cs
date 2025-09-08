using CozyCafe.Models.Domain.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CozyCafe.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// (UA) Контролер для адмінської панелі CozyCafe, який керує елементами меню.
    /// - Доступ тільки для користувачів з роллю Admin.
    /// - Index(): отримує та відображає всі елементи меню з можливістю фільтрації.
    /// - Create() [GET]: формує форму створення нового елемента меню, завантажує список категорій.
    /// - Create(dto) [POST]: додає новий елемент меню після валідації моделі.
    /// - Edit(id) [GET]: завантажує дані елемента меню для редагування, формує список категорій.
    /// - Edit(id, dto) [POST]: оновлює дані елемента меню, зберігаючи попереднє зображення, якщо нове не задано.
    /// - Delete(id) [GET]: завантажує дані для підтвердження видалення.
    /// - DeleteConfirmed(id) [POST]: остаточно видаляє елемент меню з бази даних.
    ///
    /// (EN) Controller for CozyCafe admin panel managing menu items.
    /// - Access restricted to Admin role users.
    /// - Index(): fetches and displays all menu items with optional filtering.
    /// - Create() [GET]: prepares the form for creating a new menu item, loads category list.
    /// - Create(dto) [POST]: adds a new menu item after model validation.
    /// - Edit(id) [GET]: loads menu item data for editing, prepares category list.
    /// - Edit(id, dto) [POST]: updates menu item data, preserving previous image if a new one is not provided.
    /// - Delete(id) [GET]: loads data for deletion confirmation.
    /// - DeleteConfirmed(id) [POST]: permanently deletes a menu item from the database.
    /// </summary>


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
