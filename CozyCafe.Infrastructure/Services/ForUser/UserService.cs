using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Models.Domain.ForUser;
using CozyCafe.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CozyCafe.Application.Services.ForUser
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> GetByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> UpdateProfileAsync(string userId, UserProfileViewModel profile)
        {
            var user = await GetByIdAsync(userId);
            if (user == null) return false;

            user.FullName = profile.FullName;
            // якщо є інші поля — онови їх

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<IdentityResult> DeleteAccountAsync(string userId, string password)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var passwordCheck = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordCheck)
                return IdentityResult.Failed(new IdentityError { Description = "Invalid password" });

            return await _userManager.DeleteAsync(user);
        }
    }
}
