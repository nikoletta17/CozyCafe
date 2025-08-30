using Microsoft.AspNetCore.Mvc.Rendering;

namespace CozyCafe.Models.DTO.Admin
{
    public class MenuItemDto
    {
        public int Id { get; set; }

        // Основна інформація про меню
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public required string CategoryName { get; set; }

        // Для вибору категорії
        public int CategoryId { get; set; } // Зберігає вибрану категорію
        public IEnumerable<SelectListItem>? Categories { get; set; } // Список категорій для випадаючого меню
    }
}
