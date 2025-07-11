﻿using System.Collections.Generic;
using System.Linq;

namespace CozyCafe.Models.DTO.ForUser
{
    public class CartDto
    {
        public required List<CartItemDto> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.Price * i.Quantity);
    }
}
