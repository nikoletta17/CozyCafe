using System.Runtime.CompilerServices;
using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.DTO.Admin;
using CozyCafe.Web.Areas.User.Controllers.Generic_Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Areas.User.Controllers
{
    [Area("User")]
    [Route("User/[controller]/[action]")]
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
            // Отримуємо всі категорії з вказаним ParentCategoryId (може бути null для кореневих категорій)
            var categories = await _categoryService.GetByParentCategoryIdAsync(parentCategoryId);

            // Мапимо отримані доменні моделі у DTO для передачі у View
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            // Повертаємо View "Index", передаючи у нього список категорій у вигляді DTO
            return View("Index", dtos);
        }


    }
}
