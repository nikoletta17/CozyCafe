using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Models.DTO.Admin
{
    public class DashboardStatsDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalUsers { get; set; }
        public List<TopMenuItemDto> TopMenuItems { get; set; } = new();
    }

    public class TopMenuItemDto
    {
        public string Name { get; set; } = null!; // Ім'я меню
        public int QuantitySold { get; set; }      // Кількість проданих одиниць
    }
}
