using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Application.Interfaces.Generic_Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity); // бо EF має AddAsync()
        void Update(T entity);   // бо EF не має UpdateAsync()
        void Delete(T entity);   // бо EF не має DeleteAsync()
        Task SaveChangesAsync();

    }
}
