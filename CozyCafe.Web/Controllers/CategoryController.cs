using System.Runtime.CompilerServices;
using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices;
using CozyCafe.Models.Domain;
using CozyCafe.Web.Controllers.Generic_Controller;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Controllers
{
    public class CategoryController : GenericController<Category>
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper): base(categoryService)
        {
            _categoryService = categoryService;
            _mapper = mapper;   
        }

        public async Task<IActionResult> ByParentCategory(int? parentCategoryId)
        {
            var parentCategory = await _categoryService.GetByParentCategoryIdAsync(parentCategoryId);
            var dtos = _mapper.Map<IEnumerable<Category>>(parentCategory);
            return View("Index", dtos);
        }
    }
}
