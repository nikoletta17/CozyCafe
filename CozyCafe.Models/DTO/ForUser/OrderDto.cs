using System;
using System.Collections.Generic;

namespace CozyCafe.Models.DTO.ForUser
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderedAt { get; set; }
        public decimal Total { get; set; }
        public required string Status { get; set; }
        public string? DiscountCode { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public required string MenuItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<OrderItemOptionDto> SelectedOptions { get; set; } = new();
    }
}
