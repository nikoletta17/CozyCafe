using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Models.DTO
{
    public class DiscountDto
    {
        public string Code { get; set; }
        public int Percent { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
