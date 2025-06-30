using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CozyCafe.Models.DTO
{
    public class CreateOrderDto
    {
        public string? DiscountCode { get; set; }

        [Required]
        public required List<CreateOrderItemDto> Items { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        [Required]
        public int MenuItemId { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
        public List<int> SelectedOptionIds { get; set; } = new();
    }
}
