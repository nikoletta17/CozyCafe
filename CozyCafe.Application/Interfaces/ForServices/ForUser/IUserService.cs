using CozyCafe.Models.Domain.ForUser; // або твій простір імен для ApplicationUser
using CozyCafe.Models.ViewModels;

using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CozyCafe.Application.Interfaces.ForServices.ForUser
{
    public interface IUserService
    {
        Task<ApplicationUser?> GetByIdAsync(string userId);
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<bool> UpdateProfileAsync(string userId, UserProfileViewModel profile);

        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<IdentityResult> DeleteAccountAsync(string userId, string password);
    }
}
