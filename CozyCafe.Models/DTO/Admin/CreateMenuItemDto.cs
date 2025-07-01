using System.ComponentModel.DataAnnotations;

namespace CozyCafe.Models.DTO.Admin
{
    public class CreateMenuItemDto
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, 9999.99)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
