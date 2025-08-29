using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Application.Interfaces.Generic_Interfaces
{
    /// <summary>
    /// (UA) Узагальнений інтерфейс сервісу для бізнес-логіки над будь-якими сутностями. 
    /// Використовує репозиторій і надає стандартні CRUD-операції у асинхронному режимі. 
    /// Дозволяє ізолювати контролери від прямої роботи з даними.
    /// 
    /// (EN) Generic service interface for business logic over any entities. 
    /// Uses a repository and provides standard CRUD operations asynchronously. 
    /// Helps isolate controllers from direct data access.
    /// </summary>
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
