using System.ComponentModel.DataAnnotations;

namespace CozyCafe.Models.ViewModels
{
    public class UserProfileViewModel
    {
        [Required(ErrorMessage = "Повне ім'я обов'язкове")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Некоректний Email")]
        public string Email { get; set; } = string.Empty;
    }
}
