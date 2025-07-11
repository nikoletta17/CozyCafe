﻿using System.ComponentModel.DataAnnotations;

namespace CozyCafe.Models.Domain.Admin
{
    public class MenuItemOptionGroup
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; } // Наприклад: "Сиропи", "Інгредієнти", "Соуси"

        [Required]
        public int MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }

        public ICollection<MenuItemOption> Options { get; set; } = new List<MenuItemOption>();
    }
}
