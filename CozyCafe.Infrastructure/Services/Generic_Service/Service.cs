using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Generic_Interfaces;

namespace CozyCafe.Application.Services.Generic_Service
{
    // Реалізація базового сервісу

    /// <summary>
    /// (UA) Узагальнена реалізація сервісного шару для роботи з будь-якими сутностями.  
    /// Виконує роль проміжної ланки між контролером і репозиторієм, 
    /// інкапсулюючи бізнес-логіку та забезпечуючи єдиний стандарт CRUD-операцій.  
    /// Клас виконує такі завдання:
    /// - Отримання всіх сутностей або конкретної за Id.
    /// - Додавання нових сутностей із автоматичним збереженням змін.
    /// - Оновлення існуючих сутностей.
    /// - Видалення сутностей за Id із попередньою перевіркою їх існування.  
    /// Це дозволяє мінімізувати дублювання коду та забезпечує гнучке розширення для специфічних сервісів.  
    ///
    /// (EN) Generic service layer implementation for working with any entities.  
    /// Acts as a middle layer between the controller and the repository, 
    /// encapsulating business logic and providing a unified standard for CRUD operations.  
    /// The class handles:
    /// - Retrieving all entities or a specific one by Id.
    /// - Adding new entities with automatic persistence of changes.
    /// - Updating existing entities.
    /// - Deleting entities by Id with existence validation.  
    /// This minimizes code duplication and provides a flexible base for extending entity-specific services.
    /// </summary>
    public class Service<T> : IService<T> where T : class
    {
        protected readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _repository.GetAllAsync();
        public virtual async Task<T?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public virtual async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                _repository.Delete(entity);
                await _repository.SaveChangesAsync();
            }
        }
    }

}
