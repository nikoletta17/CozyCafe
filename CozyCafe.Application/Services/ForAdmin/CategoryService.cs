using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.DTO;

namespace CozyCafe.Application.Services.ForAdmin
{
    public class CategoryService: Service<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository): base(categoryRepository) 
        {
          _categoryRepository = categoryRepository;   
        }

        public async Task<IEnumerable<Category>> GetByParentCategoryIdAsync(int? parentCategoryId)
        {
            return await _categoryRepository.GetByParentCategoryIdAsync(parentCategoryId);
        }
    }
}
