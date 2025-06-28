using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Interfaces
{
    public interface ICategoryRepository: IRepository<Category>
    {
        Task<IEnumerable<Category>> GetByParentCategoryIdAsync(int? parentCategoryId);
    }
}

//ADD TO PROGRAM FILE!!!!!!!!
