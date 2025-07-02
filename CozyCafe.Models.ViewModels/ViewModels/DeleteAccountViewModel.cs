using System.ComponentModel.DataAnnotations;

namespace CozyCafe.Models.ViewModels

{
    public class DeleteAccountViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердьте свій пароль")]
        public string Password { get; set; } = string.Empty;
    }
}

