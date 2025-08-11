using AutoMapper;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Web.Areas.User.Controllers.Generic_Controller;
using Microsoft.AspNetCore.Mvc;


namespace CozyCafe.Web.Areas.User.Controllers
{
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
