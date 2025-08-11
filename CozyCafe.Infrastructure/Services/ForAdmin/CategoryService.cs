using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.Admin;

public class CategoryService : Service<Category>, ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILoggerService _logger;

    public CategoryService(ICategoryRepository categoryRepository, ILoggerService logger)
        : base(categoryRepository)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Category>> GetByParentCategoryIdAsync(int? parentCategoryId)
    {
        _logger.LogInfo($"Отримання категорій з parentCategoryId={parentCategoryId?.ToString() ?? "NULL"}.");
        var result = await _categoryRepository.GetByParentCategoryIdAsync(parentCategoryId);
        _logger.LogInfo($"Знайдено {result.Count()} категорій.");
        return result;
    }
}
