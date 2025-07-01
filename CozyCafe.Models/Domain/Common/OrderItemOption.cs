using CozyCafe.Models.Domain.Admin;

namespace CozyCafe.Models.Domain.Common
{
    public class OrderItemOption
    {
        public int Id { get; set; }

        public int OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }

        public int MenuItemOptionId { get; set; }
        public MenuItemOption? MenuItemOption { get; set; }
    }
}
