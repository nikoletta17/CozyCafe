using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Models.Domain.ForUser;
using CozyCafe.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<ApplicationUser?> GetByIdAsync(string userId)
    {
        _logger.LogInformation("Отримання користувача за Id={UserId}", userId);
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            _logger.LogWarning("Користувач Id={UserId} не знайдений", userId);
        return user;
    }

    public async Task<ApplicationUser?> GetByEmailAsync(string email)
    {
        _logger.LogInformation("Отримання користувача за email={Email}", email);
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            _logger.LogWarning("Користувач з email={Email} не знайдений", email);
        return user;
    }

    public async Task<bool> UpdateProfileAsync(string userId, UserProfileViewModel profile)
    {
        _logger.LogInformation("Оновлення профілю користувача {UserId}", userId);
        var user = await GetByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("Користувач {UserId} не знайдений для оновлення профілю", userId);
            return false;
        }

        user.FullName = profile.FullName;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
            _logger.LogInformation("Профіль користувача {UserId} успішно оновлено", userId);
        else
            _logger.LogWarning("Не вдалося оновити профіль користувача {UserId}: {Errors}", userId, string.Join("; ", result.Errors.Select(e => e.Description)));

        return result.Succeeded;
    }

    public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        _logger.LogInformation("Зміна пароля користувача {UserId}", userId);

        var user = await GetByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("Користувач {UserId} не знайдений для зміни пароля", userId);
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (result.Succeeded)
            _logger.LogInformation("Пароль користувача {UserId} успішно змінено", userId);
        else
            _logger.LogWarning("Не вдалося змінити пароль користувача {UserId}: {Errors}", userId, string.Join("; ", result.Errors.Select(e => e.Description)));

        return result;
    }

    public async Task<IdentityResult> DeleteAccountAsync(string userId, string password)
    {
        _logger.LogInformation("Видалення акаунту користувача {UserId}", userId);

        var user = await GetByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("Користувач {UserId} не знайдений для видалення акаунту", userId);
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }

        var passwordCheck = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordCheck)
        {
            _logger.LogWarning("Невірний пароль при видаленні акаунту користувача {UserId}", userId);
            return IdentityResult.Failed(new IdentityError { Description = "Invalid password" });
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
            _logger.LogInformation("Акаунт користувача {UserId} успішно видалено", userId);
        else
            _logger.LogWarning("Не вдалося видалити акаунт користувача {UserId}: {Errors}", userId, string.Join("; ", result.Errors.Select(e => e.Description)));

        return result;
    }
}
