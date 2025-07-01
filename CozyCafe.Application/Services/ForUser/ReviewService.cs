using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.ForRerository.ForUser;
using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.ForUser;

namespace CozyCafe.Application.Services.ForUser
{
    public class ReviewService: Service<Review>, IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewService(IReviewRepository reviewRepository): base(reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<Review>> GetByMenuItemIdAsync(int menuItemId)
        {
            return await _reviewRepository.GetByMenuItemIdAsync(menuItemId);
        }
        public async Task<IEnumerable<Review>> GetByUserIdAsync(string userId)
        {
            return await _reviewRepository.GetByUserIdAsync(userId);
        }
    }
}
