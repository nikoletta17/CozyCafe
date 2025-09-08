using AutoMapper;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Web.Areas.User.Controllers.Generic_Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Areas.User.Controllers
{
    /// <summary>
    /// (UA) Контролер для користувацької зони CozyCafe, який керує категоріями меню.
    /// - Спадкує базовий GenericController для стандартних CRUD-операцій.
    /// - ByParentCategory(): отримує список категорій за ParentCategoryId (доступний анонімно).
    /// - Create() [GET, Admin]: відображає форму створення нової категорії.
    /// - Create(dto) [POST, Admin]: додає нову категорію після валідації.
    /// - Edit(id) [GET, Admin]: завантажує дані категорії для редагування.
    /// - Edit(id, dto) [POST, Admin]: оновлює категорію, зберігаючи правильний Id.
    /// - Delete(id) [GET, Admin]: завантажує дані для підтвердження видалення.
    /// - DeleteConfirmed(dto) [POST]: видаляє категорію з бази даних.
    ///
    /// (EN) Controller for the CozyCafe user area managing menu categories.
    /// - Inherits from the base GenericController to handle standard CRUD operations.
    /// - ByParentCategory(): retrieves a list of categories by ParentCategoryId (available to anonymous users).
    /// - Create() [GET, Admin]: displays the form for creating a new category.
    /// - Create(dto) [POST, Admin]: adds a new category after validation.
    /// - Edit(id) [GET, Admin]: loads category data for editing.
    /// - Edit(id, dto) [POST, Admin]: updates the category, ensuring the correct Id is preserved.
    /// - Delete(id) [GET, Admin]: loads data for deletion confirmation.
    /// - DeleteConfirmed(dto) [POST]: removes the category from the database.
    /// </summary>


    [Area("User")]
    [Route("User/[controller]/[action]")]
    public class CategoryController : GenericController<Category>
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, IMapper mapper, ILogger<CategoryController> logger)
            : base(categoryService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> ByParentCategory(int? parentCategoryId = null)
        {
            _logger.LogInformation("Отримання категорій з ParentCategoryId = {ParentCategoryId}", parentCategoryId);

            var categories = await _categoryService.GetByParentCategoryIdAsync(parentCategoryId);
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            _logger.LogInformation("Отримано {Count} категорій", dtos.Count());

            return View("Index", dtos);
        }


        // --- Лише для Адміністраторів ---
        // --- CREATE ---
        [Authorize(Roles = "Admin")]
        [HttpGet("Create")]
        public IActionResult Create() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDto dto)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(dto);
                await _categoryService.AddAsync(category);
                return RedirectToAction(nameof(ByParentCategory));
            }
            return View(dto);
        }

        // --- EDIT ---
        [Authorize(Roles = "Admin")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            var dto = _mapper.Map<CategoryDto>(category);
            return View(dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryDto dto)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(dto);
                category.Id = id; // гарантія правильного Id
                await _categoryService.UpdateAsync(category);
                return RedirectToAction(nameof(ByParentCategory));
            }
            return View(dto);
        }

        // --- DELETE ---
        [Authorize(Roles = "Admin")]
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            var dto = _mapper.Map<CategoryDto>(category);
            return View(dto);
        }

        [HttpPost]
        [ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(CategoryDto dto)
        {
            if (dto == null) return BadRequest();

            await _categoryService.DeleteAsync(dto.Id);
            return RedirectToAction(nameof(ByParentCategory));
        }

    }
}
