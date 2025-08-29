using CozyCafe.Application.Exceptions;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.Admin;
using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// (UA) Сервіс для роботи з категоріями у адміністративній частині CozyCafe.  
/// Наслідує базовий сервіс `Service<Category>` і реалізує додаткову логіку:  
/// - Отримання категорій за батьківською категорією з використанням кешування (MemoryCache).  
/// - Автоматичне очищення кешу при додаванні, оновленні або видаленні категорії.  
/// - Логування усіх операцій та кеш-дій для спрощеного відстеження.  
/// Використовується для підвищення продуктивності та підтримки актуальності даних.
/// 
/// (EN) Service for managing categories in the CozyCafe admin area.  
/// Inherits the base `Service<Category>` and implements additional logic:  
/// - Retrieving categories by parent category with caching (MemoryCache).  
/// - Automatic cache clearing on add, update, or delete operations.  
/// - Logging of all operations and cache actions for easy tracking.  
/// Improves performance and ensures data consistency.
/// </summary>
public class CategoryService : Service<Category>, ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILoggerService _logger;
    private readonly IMemoryCache _cache;

    private const string AllCategoriesCacheKey = "AllCategories";
    private const string ParentCategoriesCacheKeyPrefix = "Categories_Parent_";

    public CategoryService(
        ICategoryRepository categoryRepository,
        ILoggerService logger,
        IMemoryCache cache)
        : base(categoryRepository)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
        _cache = cache;
    }

    public async Task<IEnumerable<Category>> GetByParentCategoryIdAsync(int? parentCategoryId)
    {
        string cacheKey = $"{ParentCategoriesCacheKeyPrefix}{parentCategoryId?.ToString() ?? "NULL"}";

        if (!_cache.TryGetValue(cacheKey, out IEnumerable<Category> categories))
        {
            _logger.LogInfo($"[CACHE MISS] Отримання категорій з parentCategoryId={parentCategoryId?.ToString() ?? "NULL"}.");
            categories = await _categoryRepository.GetByParentCategoryIdAsync(parentCategoryId);

            if (!categories.Any())
            {
                _logger.LogWarning($"Категорії з parentCategoryId={parentCategoryId} не знайдено.");
                throw new NotFoundException("Category", parentCategoryId ?? 0);
            }

            _cache.Set(cacheKey, categories, TimeSpan.FromMinutes(10));
            _logger.LogInfo($"[CACHE SET] Збережено {categories.Count()} категорій у кеш з ключем {cacheKey}.");
        }
        else
        {
            _logger.LogInfo($"[CACHE HIT] Категорії з ключем {cacheKey} отримані з кешу.");
        }

        return categories;
    }

    public override async Task AddAsync(Category entity)
    {
        await base.AddAsync(entity);
        ClearCache();
    }

    public override async Task UpdateAsync(Category entity)
    {
        await base.UpdateAsync(entity);
        ClearCache();
    }

    public override async Task DeleteAsync(int id)
    {
        await base.DeleteAsync(id);
        ClearCache();
    }

    private void ClearCache()
    {
        _logger.LogInfo("[CACHE CLEAR] Очищення кешу категорій.");
        _cache.Remove(AllCategoriesCacheKey);
    }
}
