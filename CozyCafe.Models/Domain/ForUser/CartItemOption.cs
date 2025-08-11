using CozyCafe.Models.Domain.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Models.Domain.ForUser
{
    public class CartItemOption
    {
        public int Id { get; set; }

        public int CartItemId { get; set; }
        public CartItem? CartItem { get; set; }

        public int MenuItemOptionId { get; set; }
        public MenuItemOption? MenuItemOption { get; set; }
    }
}
