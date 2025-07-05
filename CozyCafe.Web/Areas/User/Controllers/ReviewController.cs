using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.ForUser;
using CozyCafe.Models.DTO.Admin;
using CozyCafe.Models.DTO.ForUser;
using CozyCafe.Web.Areas.User.Controllers.Generic_Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace CozyCafe.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize] 
    [Route("User/[controller]/[action]")]
    public class ReviewController : GenericController<Review>
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;
        private readonly IMenuItemService _menuItemService;

        public ReviewController(IReviewService reviewService, IMapper mapper, IMenuItemService menuItemService) : base(reviewService)
        {
            _reviewService = reviewService;
            _mapper = mapper;
            _menuItemService = menuItemService;
        }


        // All reviews for the certain MenuItem
        public async Task<IActionResult> ByMenuItem(int menuItemId)
        {
            var reviews = await _reviewService.GetByMenuItemIdAsync(menuItemId);
            var dto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            ViewBag.MenuItemId = menuItemId; // 👈 передаємо в представлення

            return View("ReviewsByMenuItem", dto);
        }


        // All reviews from a specific user
        public async Task<IActionResult> ByUser(string userId)
        {
            var reviews = await _reviewService.GetByUserIdAsync(userId);
            var dto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
            return View("ReviewsByUser", dto);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create(int? menuItemId)
        {
            var menuItems = await _menuItemService.GetFilteredAsync(new MenuItemFilterModel());

            ViewBag.MenuItems = menuItems.Select(mi => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = mi.Id.ToString(),
                Text = mi.Name,
                Selected = (mi.Id == menuItemId)
            }).ToList();

            var model = new CreateReviewDto();

            if (menuItemId.HasValue)
                model.MenuItemId = menuItemId.Value;

            return View(model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewDto dto)
        {
            if (!ModelState.IsValid)
            {
                var menuItems = await _menuItemService.GetFilteredAsync(new MenuItemFilterModel());

                ViewBag.MenuItems = menuItems.Select(mi => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = mi.Id.ToString(),
                    Text = mi.Name,
                    Selected = (mi.Id == dto.MenuItemId)
                }).ToList();

                return View(dto);
            }

            var review = _mapper.Map<Review>(dto);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "Не вдалося визначити користувача.");

                var menuItems = await _menuItemService.GetFilteredAsync(new MenuItemFilterModel());

                ViewBag.MenuItems = menuItems.Select(mi => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = mi.Id.ToString(),
                    Text = mi.Name,
                    Selected = (mi.Id == dto.MenuItemId)
                }).ToList();

                return View(dto);
            }

            review.UserId = userId;
            review.CreatedAt = DateTime.UtcNow;

            await _reviewService.AddAsync(review);

            return RedirectToAction("ByMenuItem", new { menuItemId = dto.MenuItemId });
        }

        [AllowAnonymous] // Якщо хочеш, щоб навіть неавторизовані бачили відгуки
        public async Task<IActionResult> All()
        {
            var reviews = await _reviewService.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
            return View("AllReviews", dto); // або "Index" — залежно від назви твого View
        }

    }
}
