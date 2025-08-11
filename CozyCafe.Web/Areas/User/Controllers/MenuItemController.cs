using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;


namespace CozyCafe.Web.Areas.User.Controllers
{
    [Area("User")]
    [Route("User/[controller]")]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        private readonly ICategoryService _categoryService;
        private readonly IMenuItemOptionGroupService _optionGroupService;
        private readonly ILogger<MenuItemController> _logger;

        public MenuItemController(
            IMenuItemService menuItemService,
            ICategoryService categoryService,
            IMenuItemOptionGroupService optionGroupService,
            ILogger<MenuItemController> logger)
        {
            _menuItemService = menuItemService;
            _categoryService = categoryService;
            _optionGroupService = optionGroupService;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(MenuItemFilterModel filter)
        {
            _logger.LogInformation("Запит меню з фільтром: {@Filter}", filter);

            // Захист пагінації
            filter.Page = filter.Page < 1 ? 1 : filter.Page;
            filter.PageSize = filter.PageSize <= 0 ? 8 : filter.PageSize;

            // Якщо користувач передав CategoryName — переведемо у CategoryId
            if (!string.IsNullOrWhiteSpace(filter.CategoryName))
            {
                var categories = await _categoryService.GetAllAsync();
                var category = categories.FirstOrDefault(c => c.Name == filter.CategoryName);
                if (category != null)
                {
                    filter.CategoryId = category.Id;
                    _logger.LogInformation("CategoryName \"{CategoryName}\" -> CategoryId {CategoryId}", filter.CategoryName, category.Id);
                }
            }

            // Нормалізація CategoryId та цін
            if (filter.CategoryId.HasValue && filter.CategoryId.Value <= 0)
                filter.CategoryId = null;

            if (!filter.MinPrice.HasValue || filter.MinPrice <= 0)
                filter.MinPrice = null;

            if (!filter.MaxPrice.HasValue || filter.MaxPrice <= 0)
                filter.MaxPrice = null;

            // Отримуємо вже відфільтрований набір (з сервісу)
            var items = await _menuItemService.GetFilteredAsync(filter);

            // Лог для відлагодження — перевірити чи SortBy доходить
            _logger.LogInformation("Requested SortBy = {SortBy}", filter.SortBy);

            // Приведемо до list щоб можна було застосувати OrderBy без залежності від реалізації сервісу
            var itemsList = items.ToList();

            // Застосуємо сортування на рівні контролера (тимчасове швидке рішення)
            itemsList = (filter.SortBy ?? string.Empty).ToLower() switch
            {
                "price_asc" => itemsList.OrderBy(i => i.Price).ToList(),
                "price_desc" => itemsList.OrderByDescending(i => i.Price).ToList(),
                "name" => itemsList.OrderBy(i => i.Name).ToList(),
                _ => itemsList // дефолтне сортування — як є
            };

            // Пагінація
            int totalItems = itemsList.Count;
            filter.TotalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);

            var pagedItems = itemsList
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .AsEnumerable();

            // Підготовка ViewBag
            var allCategories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(allCategories, "Id", "Name", filter.CategoryId);

            var sortOptions = new List<SelectListItem>
    {
        new SelectListItem { Value = "", Text = "За замовчуванням" },
        new SelectListItem { Value = "name", Text = "Назва" },
        new SelectListItem { Value = "price_asc", Text = "Ціна ↑" },
        new SelectListItem { Value = "price_desc", Text = "Ціна ↓" },
    };
            foreach (var option in sortOptions)
                option.Selected = option.Value == (filter.SortBy ?? "");

            ViewBag.SortOptions = sortOptions;

            _logger.LogInformation("Підготовлено {Count} товарів для відображення з фільтром {@Filter}", pagedItems.Count(), filter);

            return View((pagedItems, filter));
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Запит деталей меню для Id = {Id}", id);

            var item = await _menuItemService.GetByIdAsync(id);
            if (item == null)
            {
                _logger.LogWarning("Товар меню з Id = {Id} не знайдено", id);
                return NotFound();
            }

            var groups = await _optionGroupService.GetByMenuItemIdAsync(id);
            var optionGroupDtos = groups.Select(g => new MenuItemOptionGroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Options = g.Options.Select(o => new MenuItemOptionDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    ExtraPrice = o.ExtraPrice
                }).ToList()
            }).ToList();

            ViewBag.OptionGroups = optionGroupDtos;

            return View("Details", item);
        }
    }
}
