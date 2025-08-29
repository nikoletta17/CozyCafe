using CozyCafe.Application.Exceptions;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.DTO.Admin;
using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// (UA) Сервіс для роботи з елементами меню у адміністративній частині CozyCafe.  
/// Наслідує базовий сервіс `Service<MenuItem>` і додає кешування та логування:  
/// - Отримання елементів меню за фільтром з кешем (MemoryCache).  
/// - Отримання елемента меню за ID з кешем.  
/// - Автоматичне очищення кешу при додаванні, оновленні або видаленні елемента.  
/// - Логування операцій та дій з кешем для контролю процесу.  
/// Використовується для підвищення продуктивності та підтримки актуальності даних.
///
/// (EN) Service for managing menu items in the CozyCafe admin area.  
/// Inherits the base `Service<MenuItem>` and adds caching and logging:  
/// - Retrieve menu items by filter using cache (MemoryCache).  
/// - Retrieve menu item by ID using cache.  
/// - Automatic cache clearing on add, update, or delete operations.  
/// - Logs operations and cache actions to monitor the process.  
/// Improves performance and ensures data consistency.
/// </summary>
public class MenuItemService : Service<MenuItem>, IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly ILoggerService _logger;
    private readonly IMemoryCache _cache;

    private const string MenuItemCacheKeyPrefix = "MenuItem_";
    private const string FilteredMenuCacheKeyPrefix = "MenuItems_Filter_";

    public MenuItemService(IMenuItemRepository menuItemRepository,
                           ILoggerService logger,
                           IMemoryCache cache)
        : base(menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
        _logger = logger;
        _cache = cache;
    }

    public async Task<IEnumerable<MenuItemDto>> GetFilteredAsync(MenuItemFilterModel filterModel)
    {
        string cacheKey = $"{FilteredMenuCacheKeyPrefix}{filterModel.CategoryId}_{filterModel.SearchTerm}_{filterModel.MinPrice}_{filterModel.MaxPrice}";

        if (!_cache.TryGetValue(cacheKey, out IEnumerable<MenuItemDto> cachedItems))
        {
            _logger.LogInfo($"[CACHE MISS] Фільтрація MenuItems з ключем {cacheKey}");
            var items = await _menuItemRepository.GetFilteredAsync(filterModel);

            if (!items.Any())
            {
                _logger.LogWarning("Жодного елемента меню не знайдено за вказаним фільтром.");

                cachedItems = new List<MenuItemDto>(); 
                _cache.Set(cacheKey, cachedItems, TimeSpan.FromMinutes(10));
                return cachedItems;
            }


            cachedItems = items.Select(mi => new MenuItemDto
            {
                Id = mi.Id,
                Name = mi.Name,
                Description = mi.Description,
                Price = mi.Price,
                ImageUrl = mi.ImageUrl,
                CategoryName = mi.Category?.Name ?? ""
            }).ToList();

            _cache.Set(cacheKey, cachedItems, TimeSpan.FromMinutes(10));
            _logger.LogInfo($"[CACHE SET] Збережено {cachedItems.Count()} елементів у кеш.");
        }
        else
        {
            _logger.LogInfo($"[CACHE HIT] Отримано дані з кешу для ключа {cacheKey}");
        }

        return cachedItems;
    }

    public async Task<MenuItemDto> GetByIdAsync(int id)
    {
        string cacheKey = $"{MenuItemCacheKeyPrefix}{id}";

        if (!_cache.TryGetValue(cacheKey, out MenuItemDto cachedItem))
        {
            _logger.LogInfo($"[CACHE MISS] Отримання MenuItem Id={id}");
            var item = await _menuItemRepository.GetByIdWithCategoryAsync(id);

            if (item == null)
            {
                _logger.LogWarning($"MenuItem Id={id} не знайдено");
                throw new MenuItemNotFoundException(id);
            }

            cachedItem = new MenuItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                ImageUrl = item.ImageUrl,
                CategoryName = item.Category?.Name ?? "Без категорії"
            };

            _cache.Set(cacheKey, cachedItem, TimeSpan.FromMinutes(10));
            _logger.LogInfo($"[CACHE SET] MenuItem Id={id} збережено у кеш.");
        }
        else
        {
            _logger.LogInfo($"[CACHE HIT] MenuItem Id={id} з кешу.");
        }

        return cachedItem;
    }

    public override async Task AddAsync(MenuItem entity)
    {
        await base.AddAsync(entity);
        ClearCache();
    }

    public override async Task UpdateAsync(MenuItem entity)
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
        _logger.LogInfo("[CACHE CLEAR] Очищення кешу MenuItems.");
    }
}
