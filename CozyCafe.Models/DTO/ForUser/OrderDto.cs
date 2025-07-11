﻿using System;
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
        public required string UserId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int Id { get; set; }  // Унікальний ідентифікатор пункту замовлення
        public int MenuItemId { get; set; }

        public required string MenuItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<OrderItemOptionDto> SelectedOptions { get; set; } = new();
    }

    public class OrderItemOptionDto
    {
        public int Id { get; set; } // Унікальний ідентифікатор опції
        public required string Name { get; set; }
        public decimal? ExtraPrice { get; set; }
    }
}
