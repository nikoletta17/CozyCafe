using System.Collections.Generic;
using System.Linq; 

namespace CozyCafe.Models.DTO
{
    public class CartDto
    {
        public List<CartItemDto> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.Price * i.Quantity);
    }
}
