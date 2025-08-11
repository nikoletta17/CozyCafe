using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.Admin;
using Microsoft.Extensions.Caching.Memory;

public class CategoryService : Service<Category>, ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILoggerService _logger;
    private readonly IMemoryCache _cache;

    // Ключі кешу для списку і для пошуку по ParentCategoryId
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
        // Видаляємо всі кеші по ParentCategoryId
        // (Тут можна розширити, якщо тримаєш список ключів)
    }
}
