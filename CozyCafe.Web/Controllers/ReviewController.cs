using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.ForUser;
using CozyCafe.Models.DTO.ForUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace CozyCafe.Web.Controllers.Generic_Controller
{
    [Authorize]
    public class ReviewController : GenericController<Review>
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;
        public ReviewController(IReviewService reviewService, IMapper mapper) :base(reviewService) 
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        // All reviews for the certain MenuItem
        public async Task<IActionResult> ByMetuItem(int menuItemId)
        {
            var reviews = await _reviewService.GetByMenuItemIdAsync(menuItemId);
            var dto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
            return View("ReviewsByMenuItem", dto);
        }

        // All reviews from a specific user
        public async Task<IActionResult> ByUser(string userId)
        {
            var reviews = await _reviewService.GetByUserIdAsync(userId);
            var dto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
            return View("ReviewsByUser", dto);
        }

        // GET: New review
        [HttpGet]
        public IActionResult Create(int menuItemId)
        {
            var model = new CreateReviewDto { MenuItemId = menuItemId };
            return View(model);
        }

        // POST: Send the form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewDto dto)
        {
            if(ModelState.IsValid)
            {
                return View(dto);
            }
            var review = _mapper.Map<Review>(dto);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            review.UserId = userId; 
            review.CreatedAt = DateTime.UtcNow;

            await _reviewService.AddAsync(review);
            return RedirectToAction("ByMenuItem", new { menuItemId = dto.MenuItemId });

        }


    }
}
