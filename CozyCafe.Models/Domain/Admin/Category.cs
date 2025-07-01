using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CozyCafe.Models.Domain.Admin
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }

        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}

