using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Models.DTO.Admin
{
    public class AdminOrderDto
    {
        public int Id { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string? DiscountCode { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public DateTime OrderedAt { get; set; }

        public List<AdminOrderItemDto> Items { get; set; } = new();
    }

    public class AdminOrderItemDto
    {
        public string MenuItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<string> SelectedOptions { get; set; } = new();
    }
}
