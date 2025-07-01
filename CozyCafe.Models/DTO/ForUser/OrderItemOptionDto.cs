using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Models.DTO.ForUser
{
    public class OrderItemOptionDto
    {
        public required string Name { get; set; }
        public decimal? ExtraPrice { get; set; }
    }
}
