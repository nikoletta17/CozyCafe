using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Application.Interfaces.Generic_Interfaces
{
    /// <summary>
    /// (UA) Узагальнений інтерфейс репозиторію для роботи з будь-якими сутностями. 
    /// Визначає базові CRUD-операції та збереження змін у базі даних. 
    /// Використовується як абстракція над доступом до даних.
    /// 
    /// (EN) Generic repository interface for working with any entities. 
    /// Defines basic CRUD operations and saving changes to the database. 
    /// Used as an abstraction over data access.
    /// </summary>
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity); 
        void Update(T entity);   
        void Delete(T entity);   
        Task SaveChangesAsync();

    }
}
