using AutoMapper;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Web.Areas.User.Controllers.Generic_Controller;
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

        public async Task<IActionResult> ByParentCategory(int? parentCategoryId)
        {
            _logger.LogInformation("Отримання категорій з ParentCategoryId = {ParentCategoryId}", parentCategoryId);

            var categories = await _categoryService.GetByParentCategoryIdAsync(parentCategoryId);
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            _logger.LogInformation("Отримано {Count} категорій", dtos.Count());

            return View("Index", dtos);
        }
    }
}
