using System.ComponentModel.DataAnnotations;
using CozyCafe.Models.Domain.Common;

namespace CozyCafe.Models.Domain.Admin
{
    public class MenuItemOption
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // Наприклад: "Карамельний", "Курка", "Без сиру"

        public decimal? ExtraPrice { get; set; } // Додаткова ціна, якщо є

        [Required]
        public int OptionGroupId { get; set; }
        public MenuItemOptionGroup? OptionGroup { get; set; }

        public ICollection<OrderItemOption> OrderItemOptions { get; set; } = new List<OrderItemOption>();
    }
}
