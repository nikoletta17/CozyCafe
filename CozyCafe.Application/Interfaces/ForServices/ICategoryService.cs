using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository;
using CozyCafe.Application.Interfaces.Generic_Interfaces;
using CozyCafe.Models.Domain;

namespace CozyCafe.Application.Interfaces.ForServices
{
    public interface ICategoryService: IService<Category>
    {
        Task<IEnumerable<Category>> GetByParentCategoryIdAsync(int? parentCategoryId);
    }
}
