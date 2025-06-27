using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CozyCafe.Models.Domain
{
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
