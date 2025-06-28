using System.ComponentModel.DataAnnotations;

namespace CozyCafe.Models.Domain
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        [Required]
        public int MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, 9999.99)]
        public decimal Price { get; set; }
        public ICollection<OrderItemOption> SelectedOptions { get; set; } = new List<OrderItemOption>();

    }
}
