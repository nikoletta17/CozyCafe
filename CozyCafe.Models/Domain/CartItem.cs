using System.ComponentModel.DataAnnotations;

namespace CozyCafe.Models.Domain
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }
        public Cart? Cart { get; set; }

        [Required]
        public int MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}
