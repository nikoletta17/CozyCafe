using System.Collections.Generic;
using System.Linq;

namespace CozyCafe.Models.DTO.ForUser
{
    public class CartDto
    {
        public List<CartItemDto> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}
