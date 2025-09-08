using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CozyCafe.Models.DTO.Admin
{
    public class MenuItemDto
    {
        public int Id { get; set; }

        // Основна інформація про меню
        [Required(ErrorMessage = "Назва є обов’язковою.")]
        [StringLength(100, ErrorMessage = "Назва не може перевищувати 100 символів.")]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Опис не може перевищувати 500 символів.")]
        public string? Description { get; set; }

        [Range(0.01, 10000, ErrorMessage = "Ціна повинна бути більшою за 0.")]
        public decimal Price { get; set; }

        [Display(Name = "URL картинки")]
        public string? ImageUrl { get; set; }


        // Для категорії
        [Required(ErrorMessage = "Будь ласка, виберіть категорію.")]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        // Для вибору категорії
        public IEnumerable<SelectListItem>? Categories { get; set; } // Список категорій для випадаючого меню
    }
}
