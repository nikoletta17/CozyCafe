using AutoMapper;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Web.Areas.User.Controllers.Generic_Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Areas.User.Controllers
{
    /// <summary>
    /// (UA) Контролер для роботи з категоріями.
    /// - Наслідує GenericController<Category>.
    /// - ByParentCategory(int? parentCategoryId): Отримує категорії за батьківською категорією; повертає список CategoryDto.
    /// - Використовує ICategoryService, IMapper та ILogger для бізнес-логіки та логування.
    /// 
    /// (EN) Controller for handling categories.
    /// - Inherits GenericController<Category>.
    /// - ByParentCategory(int? parentCategoryId): Retrieves categories by parent category; returns a list of CategoryDto.
    /// - Uses ICategoryService, IMapper, and ILogger for business logic and logging.
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
