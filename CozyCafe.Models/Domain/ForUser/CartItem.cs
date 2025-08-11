using System.ComponentModel.DataAnnotations;
using CozyCafe.Models.Domain.Admin;

namespace CozyCafe.Models.Domain.ForUser
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
        public ICollection<CartItemOption> SelectedOptions { get; set; } = new List<CartItemOption>();
    }
}

