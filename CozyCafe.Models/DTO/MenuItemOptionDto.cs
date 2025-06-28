using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Models.DTO
{
    public class MenuItemOptionDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? ExtraPrice { get; set; }
    }
}
