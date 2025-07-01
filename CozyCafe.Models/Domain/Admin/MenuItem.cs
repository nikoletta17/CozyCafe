using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CozyCafe.Models.Domain.Common;
using CozyCafe.Models.Domain.ForUser;

namespace CozyCafe.Models.Domain.Admin
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, 9999.99)]
        public decimal Price { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<MenuItemOptionGroup> OptionGroups { get; set; } = new List<MenuItemOptionGroup>();

    }
}
